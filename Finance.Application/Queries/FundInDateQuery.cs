namespace Finance.Application.Queries;

public record FundInDateQuery(
    string UserId,
    int? Year,
    int? Month,
    int? Day,
    FundFilterQuery? FundFilterQuery = null);
