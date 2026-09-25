using Finance.Application.Dtos;
using Finance.Application.Responses;
using Finance.Domain.Entities;
using Finance.Domain.Enums;

namespace Finance.Application.Abstractions;

public interface IFundRepository
{
    Task CreateAsync(Fund fund);
    Task DeleteAsync(int fundId);
    Task<bool> DoesFundBelongToUserAsync(int fundId, string userId);
    Task<IEnumerable<FundDto>> GetAllAsync(string userId, FundFilterDto? dto = null);
    Task<IEnumerable<FundDto>> GetAllAsync(string userId, DateOnly from, DateOnly to, FundFilterDto? dto = null);
    Task<DetailedFundDto?> GetAsync(int fundId);
    Task<DateOnly> GetDateAsync(int fundId);
    Task<IEnumerable<FinancialFundInformationResponse>> GetFinancialFundInformationAsync(string userId);
    Task<IEnumerable<FinancialInformationResponse>> GetFinancialFundInformationAsync(string userId, FundType fundType);
    Task UpdateAsync(UpdateFundDto dto);
}
