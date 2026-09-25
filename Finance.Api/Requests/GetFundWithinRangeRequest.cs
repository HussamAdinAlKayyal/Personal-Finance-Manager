namespace Finance.Api.Requests;

public record GetFundWithinRangeRequest(
    DateOnly? From,
    DateOnly? To,
    FundFilter? FundFilter);