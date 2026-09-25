namespace Finance.Api.Requests;

public record LoginRequest(
    string Email,
    string Password);
