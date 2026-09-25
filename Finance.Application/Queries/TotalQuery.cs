using Finance.Domain.Enums;

namespace Finance.Application.Queries;

public record TotalQuery(
    string UserId, 
    int CurrencyId, 
    string? FundType);
