namespace Finance.Application.Queries;

public record FundWithinRangeQuery(
    string UserId,
    DateOnly? From,
    DateOnly? To,
    FundFilterQuery? FundFilterQuery = null);
