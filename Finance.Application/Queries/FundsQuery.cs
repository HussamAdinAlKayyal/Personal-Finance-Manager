using Finance.Application.Dtos;

namespace Finance.Application.Queries;

public record FundsQuery(
    string UserId,
    FundFilterQuery? FundFilterQuery = null);
