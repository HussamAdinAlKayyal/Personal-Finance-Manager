namespace Finance.Api.Requests;

public record RegistrationRequest(
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    string Email,
    string Password);
