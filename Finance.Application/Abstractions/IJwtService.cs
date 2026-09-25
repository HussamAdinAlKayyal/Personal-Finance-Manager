namespace Finance.Application.Abstractions;

public interface IJwtService
{
    Task<string> GenerateTokenAsync(string userId, string email);
}
