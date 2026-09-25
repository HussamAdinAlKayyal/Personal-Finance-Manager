namespace Finance.Api.Requests;

public record GetFundInDateRequest(
    int? Year,
    int? Month,
    int? Day,
    FundFilter? FundFilter);
