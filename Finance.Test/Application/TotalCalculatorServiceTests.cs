using Finance.Application.Abstractions;
using Finance.Application.Responses;
using Finance.Application.Services;
using Finance.Domain.Entities;
using Finance.Domain.Enums;
using Moq;

namespace Finance.Test.Application;

using Pair = (decimal amount, FundType fundType);

public class TotalCalculatorServiceTests
{
    private static async Task CreateFundAsync(IFundRepository repository, decimal exchangeRateInUsd, decimal amount, FundType fundType)
    {
        int currencyId = 1;
        await repository.CreateAsync(new(fundType, amount, currencyId, new(), exchangeRateInUsd, null, "1", 1));
    }

    public static IEnumerable<(Pair[] pairs, decimal exchangeRateInUsd, int currencyId, decimal total)> TestCases()
    {
        yield return (
            new Pair[] {
                new(10, FundType.Income),
                new(20, FundType.Expense),
                new(30, FundType.Income),
            },
            0.1m,
            2,
            4
        );
        yield return (
            new Pair[] {
                new(10, FundType.Income),
                new(40, FundType.Expense),
            },
            0.3m,
            2,
            -18
        );
        yield return (
            new Pair[] {
                new(160, FundType.Income),
                new(50, FundType.Expense),
                new(10, FundType.Expense),
            },
            0.5m,
            3,
            150
        );
        yield return (
            new Pair[] {
                new(43, FundType.Expense),
                new(57, FundType.Expense),
                new(10, FundType.Income),
                new(29, FundType.Expense),
                new(86, FundType.Income),
            },
            0.75m,
            3,
            -74.25m
        );
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public static async Task CalcualteAsync_ShouldReturnsTotal(
        Pair[] pairs,
        decimal exchangeRateInUsd,
        int currencyId,
        decimal total)
    {
        string userId = "";
        Mock<IFundRepository> mockFundRepository = new();
        Mock<ICurrencyService> mockCurrencyService = new();

        List<FinancialFundInformationResponse> responses = [];
        mockFundRepository.Setup(x => x.CreateAsync(It.IsAny<Fund>())).Callback<Fund>(f => responses.Add(new(f.ExchangeRateInUsd, f.Amount, f.FundType))).Returns(Task.CompletedTask);
        mockFundRepository.Setup(x => x.GetFinancialFundInformationAsync(userId)).ReturnsAsync(responses);
        mockFundRepository.Setup(x => x.GetFinancialFundInformationAsync(userId, It.IsAny<FundType>())).Returns<string, FundType>((u, t) => Task.FromResult(responses.FindAll(x => x.FundType == t).ConvertAll(f => new FinancialInformationResponse(f.Amount, f.ExchangeRateInUsd)).AsEnumerable()));

        mockCurrencyService.Setup(x => x.ConvertFromUsdAsync(It.IsAny<decimal>(), It.IsAny<int>())).ReturnsAsync((decimal amount, int currencyId) => amount * currencyId);

        IFundRepository fundRepository = mockFundRepository.Object;
        ICurrencyService currencyService = mockCurrencyService.Object;
        
        foreach (var (amount, fundType) in pairs)
            await CreateFundAsync(fundRepository, exchangeRateInUsd, amount, fundType);
        
        TotalCalculatorService totalCalculator = new(fundRepository, currencyService);
        var result = await totalCalculator.CalculateAsync(new(userId, currencyId, null));
        Assert.Equal(total, result);
    }
}
