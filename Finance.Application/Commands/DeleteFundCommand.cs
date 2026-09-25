namespace Finance.Application.Commands;

public record DeleteFundCommand(
    int FundId,
    string UserId);
