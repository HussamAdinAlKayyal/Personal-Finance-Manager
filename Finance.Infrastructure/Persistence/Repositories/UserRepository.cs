using Finance.Application.Abstractions;
using Finance.Application.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Finance.Infrastructure.Persistence.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    private readonly AppDbContext context = context;

    public async Task<DateOnly> GetDateOfBirthAsync(string userId)
    {
        DateOnly dateOfBirth = await context.Users.Where(u => userId == u.Id).Select(u => u.DateOfBirth).FirstOrDefaultAsync();
        if (dateOfBirth == default)
        {
            throw new EntityNotFoundException("User", userId);
        }
        return dateOfBirth;
    }

    public async Task<bool> ExistsAsync(string userId)
    {
        return await context.Users.AnyAsync(u => u.Id == userId);
    }
}
