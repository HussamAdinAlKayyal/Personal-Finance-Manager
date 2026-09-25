namespace Finance.Application.Abstractions;

public interface IUserRepository
{
    public Task<bool> ExistsAsync(string userId);
    public Task<DateOnly> GetDateOfBirthAsync(string userId);
}
