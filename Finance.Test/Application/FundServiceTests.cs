using Finance.Application.Abstractions;
using Finance.Application.Commands;
using Finance.Application.Dtos;
using Finance.Application.Exceptions;
using Finance.Application.Responses;
using Finance.Application.Services;
using Finance.Domain.Entities;
using Finance.Domain.Enums;
using Moq;
using System.Threading.Tasks;

namespace Finance.Test.Application;

public static class FundServiceTests
{
    private static readonly Mock<ICurrencyRepository> mockCurrencyRepository = new();
    private static readonly Mock<IFundRepository>     mockFundRepository     = new();
    private static readonly Mock<IUserRepository>     mockUserRepository     = new();
    private static readonly Mock<ICategoryRepository> mockCategoryRepository = new();
    private static readonly Mock<ICurrencyService>    mockCurrencyService    = new();

    private static FundService CreateFundService()
    {
        return new(
            mockCurrencyService.Object,
            mockFundRepository.Object,
            mockUserRepository.Object,
            mockCategoryRepository.Object,
            mockCurrencyRepository.Object);
    }

    private static void MakeUserExist(string userId)
    {
        mockUserRepository.Setup(r => r.ExistsAsync(userId)).ReturnsAsync(true);
    }

    private static void MakeCurrencyExist(int currencyId)
    {
        mockCurrencyRepository.Setup(r => r.ExistsAsync(currencyId)).ReturnsAsync(true);
    }

    private static void MakeFundBelongsToUser(int fundId, string userId)
    {
        mockFundRepository.Setup(r => r.DoesFundBelongToUserAsync(fundId, userId)).ReturnsAsync(true);
    }

    [Fact]
    public static async Task GetAsync_GetByIdEqualsTo1_ShouldReturnsValue()
    {
        string userId = "1";
        int fundId = 1, currencyId = 1, categoryId = 1;
        MakeUserExist(userId);
        MakeFundBelongsToUser(fundId, userId);
        mockFundRepository.Setup(f => f.GetAsync(fundId)).ReturnsAsync(new DetailedFundDto(fundId, 1, currencyId, 1, new(), "", FundType.Income, 1));
        mockCurrencyRepository.Setup(x => x.GetAsync(currencyId)).ReturnsAsync(new CurrencyResponse(currencyId, "", ""));
        mockCategoryRepository.Setup(x => x.GetNameAsync(categoryId)).ReturnsAsync("");
        FundService service = CreateFundService();
        var value = await service.GetAsync(new(fundId, userId));
        Assert.NotNull(value);
        Assert.Equal(1, value.Amount);
    }

    [Fact]
    public static async Task GetAsync_NotExistFund_ShouldThrowsEntityNotFoundException()
    {
        string userId = "1";
        int fundId = 1;
        MakeUserExist(userId);
        FundService service = CreateFundService();
        await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
        {
            await service.GetAsync(new(fundId, userId));
        });
    }

    [Fact]
    public static async Task GetAsync_TryingReachOthersFund_ShouldThrowsEntityNotFoundException()
    {
        string userId = "1";
        int fundId = 1;
        MakeUserExist(userId);
        mockFundRepository.Setup(f => f.GetAsync(fundId)).ReturnsAsync(new DetailedFundDto(fundId, 1, 1, 1, new(), "", FundType.Income, 1));
        FundService service = CreateFundService();
        await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
        {
            await service.GetAsync(new(fundId, userId));
        });
    }

    [Fact]
    public static async Task GetAllAsync_ShouldReturnsMultipleValues()
    {
        MakeUserExist("1");
        mockFundRepository.Setup(r => r.GetAllAsync("1", null)).ReturnsAsync(
            [
            new(1, 1, "", new(), "", "", FundType.Expense),
            new(2, 1, "", new(), "", "", FundType.Expense),
            new(3, 1, "", new(), "", "", FundType.Expense),
            new(4, 1, "", new(), "", "", FundType.Expense),
            new(5, 1, "", new(), "", "", FundType.Expense),
            ]);
        FundService service = CreateFundService();
        var collection = await service.GetAllAsync("1");
        Assert.Equal(5, collection.Count());
    }

    [Fact]
    public static async Task GetAllAsync_TryingReachOthersFunds_ShouldReturnsZeroFund()
    {
        MakeUserExist("1");
        FundService service = CreateFundService();
        var collection = await service.GetAllAsync("1");
        Assert.Empty(collection);
    }

    [Fact]
    public static async Task CreateAsync_ValidFund_ShouldSucceed()
    {
        string userId = "";
        int currencyId = 1, categoryId = 1;
        MakeUserExist(userId);
        MakeCurrencyExist(currencyId);
        mockCategoryRepository.Setup(x => x.DoesCategoryBelongToUserAsync(categoryId, userId)).ReturnsAsync(true);
        List<Fund> funds = [];
        Fund fund = new(FundType.Income, 1, currencyId, new(), 1, null, userId, categoryId);
        mockFundRepository.Setup(x => x.CreateAsync(It.IsAny<Fund>())).Callback((Fund createdFund) => funds.Add(createdFund)).Returns(Task.CompletedTask);
        mockCurrencyService.Setup(x => x.GetExchangeRateFromUsdAsync(currencyId)).ReturnsAsync(1);
        FundService service = CreateFundService();
        await service.CreateAsync(new(fund.Amount, fund.CurrencyId, fund.Date, fund.Description, fund.UserId, fund.CategoryId, fund.FundType.ToString()));
        
        Fund createdFund = Assert.Single(funds);

        Assert.Equal(fund.Amount, createdFund.Amount);
        Assert.Equal(fund.CurrencyId, createdFund.CurrencyId);
        Assert.Equal(fund.CategoryId, createdFund.CategoryId);
        Assert.Equal(fund.FundType, createdFund.FundType);
        Assert.Equal(fund.Description, createdFund.Description);
        Assert.Equal(fund.UserId, createdFund.UserId);
        Assert.Equal(fund.Date, createdFund.Date);
    }

    [Fact]
    public static async Task DeleteAsync_ValidFund_ShouldSucceed()
    {
        string userId = "";
        int currencyId = 1, categoryId = 1, fundId = 1;
        MakeUserExist(userId);
        Fund fund = new(FundType.Income, 1, currencyId, new(), 1, null, userId, categoryId);
        List<Fund> funds = [fund];
        mockFundRepository.Setup(x => x.DeleteAsync(fundId)).Callback(() => funds.Remove(fund)).Returns(Task.CompletedTask);
        mockFundRepository.Setup(x => x.DoesFundBelongToUserAsync(fundId, userId)).ReturnsAsync(true);
        FundService service = CreateFundService();
        await service.DeleteAsync(new(fundId, userId)); 
        
        Assert.Empty(funds);
    }

    [Fact]
    public static async Task UpdateAsync_ValidFund_ShouldSucceed()
    {
        string userId = "";
        int currencyId1 = 1, categoryId1 = 1, currencyId2 = 2, categoryId2 = 2, fundId = 1;
        string fundType2 = "expense";
        MakeUserExist(userId);
        MakeCurrencyExist(currencyId1);
        MakeCurrencyExist(currencyId2);
        mockCategoryRepository.Setup(x => x.DoesCategoryBelongToUserAsync(categoryId1, userId)).ReturnsAsync(true);
        mockCategoryRepository.Setup(x => x.DoesCategoryBelongToUserAsync(categoryId2, userId)).ReturnsAsync(true);
        Fund fund = new(FundType.Income, 1, currencyId1, new(), 1, null, userId, categoryId1);
        mockFundRepository.Setup(x => x.DoesFundBelongToUserAsync(fundId, userId)).ReturnsAsync(true);
        mockFundRepository.Setup(x => x.UpdateAsync(It.IsAny<UpdateFundDto>())).Callback((UpdateFundDto dto) =>
        {
            fund.SetAmount(dto.Amount);
            fund.SetCategory(dto.CategoryId);
            fund.SetDateAndCurrencyAndExchangeRateInUsd(dto.Date, dto.CurrencyId, dto.ExchangeRateInUsd);
            fund.Description = dto.Description;
        }).Returns(Task.CompletedTask);

        mockCurrencyService.Setup(x => x.GetExchangeRateFromUsdAsync(currencyId1)).ReturnsAsync(1);
        mockCurrencyService.Setup(x => x.GetExchangeRateFromUsdAsync(It.IsAny<DateOnly>(), currencyId2)).ReturnsAsync(2);
        FundService service = CreateFundService();
        UpdateFundCommand command = new(fundId, 2, currencyId2, new(2000, 1, 1), "hello", userId, categoryId2, fundType2);
        await service.UpdateAsync(command);
        
        Assert.Equal(fund.Amount,              command.Amount);
        Assert.Equal(fund.CurrencyId,          command.CurrencyId);
        Assert.Equal(fund.CategoryId,          command.CategoryId);
        Assert.Equal(fund.Description,         command.Description);
        Assert.Equal(fund.UserId,              command.UserId);
        Assert.Equal(fund.Date,                command.Date);
        Assert.Equal(fund.FundType.ToString(), command.FundType);
    }
}
