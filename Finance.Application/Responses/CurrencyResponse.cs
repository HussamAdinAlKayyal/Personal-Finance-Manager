namespace Finance.Application.Responses;

public record CurrencyResponse(
    int Id,
    string Name,
    string Code);