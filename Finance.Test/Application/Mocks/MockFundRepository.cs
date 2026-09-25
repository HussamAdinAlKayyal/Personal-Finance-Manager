using Finance.Application.Abstractions;
using Finance.Application.Dtos;
using Finance.Application.Mappers;
using Finance.Application.Responses;
using Finance.Domain.Entities;
using Finance.Domain.Enums;

namespace Finance.Test.Application.Mocks;

internal class MockFundRepository : IFundRepository
{
    private readonly List<Fund> funds = [];
    public Task CreateAsync(Fund fund)
    {
        funds.Add(fund);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int fundId)
    {
        funds.Remove(funds.Find(f => f.Id == fundId) ?? throw new Exception());
        return Task.CompletedTask;
    }

    public Task<bool> DoesFundBelongToUserAsync(int fundId, string userId)
    {
        return Task.FromResult(funds.Find(f => f.Id == fundId && f.UserId == userId) != null);
    }

    public Task<IEnumerable<FundDto>> GetAllAsync(string userId, FundFilterDto? dto = null)
    {
        return Task.FromResult(funds.FindAll(f => f.UserId == userId).Select(f => f.ToFundDto("", "")));
    }

    public Task<IEnumerable<FundDto>> GetAllAsync(string userId, DateOnly from, DateOnly to, FundFilterDto? dto = null)
    {
        return Task.FromResult(funds.FindAll(f => f.UserId == userId).Select(f => f.ToFundDto("", "")));
    }

    public Task<DetailedFundDto?> GetAsync(int fundId)
    {
        return Task.FromResult(funds.Find(f => f.Id == fundId)?.ToDetailedFundDto());
    }

    public Task<DateOnly> GetDateAsync(int fundId)
    {
        return Task.FromResult(funds.Find(f => f.Id == fundId)?.Date ?? throw new Exception());
    }

    public Task<IEnumerable<FinancialFundInformationResponse>> GetFinancialFundInformationAsync(string userId)
    {
        return Task.FromResult(funds.FindAll(f => f.UserId == userId).Select(f => f.ToFinancialFundInformationResponse()));
    }

    public Task<IEnumerable<FinancialInformationResponse>> GetFinancialFundInformationAsync(string userId, FundType fundType)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(UpdateFundDto dto)
    {
        Fund fund = funds.Find(f => f.Id == dto.FundId) ?? throw new Exception();
        fund.SetCurrencyAndExchangeRateInUsd(dto.CurrencyId, dto.ExchangeRateInUsd);
        fund.SetDateAndExchangeRateInUsd(dto.Date, dto.ExchangeRateInUsd);
        fund.SetAmount(dto.Amount);
        fund.Description = dto.Description;
        fund.SetCategory(dto.CategoryId);
        return Task.CompletedTask;
    }
}
