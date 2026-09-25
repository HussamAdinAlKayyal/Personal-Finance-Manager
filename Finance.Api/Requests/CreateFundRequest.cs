namespace Finance.Api.Requests;

public record CreateFundRequest(
    decimal Amount,
    int CurrencyId,
    DateOnly Date,
    string? Description,
    int CategoryId,
    string FundType);
