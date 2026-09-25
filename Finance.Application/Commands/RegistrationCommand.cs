namespace Finance.Application.Commands;

public record RegistrationCommand(
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    string Email,
    string Password);
