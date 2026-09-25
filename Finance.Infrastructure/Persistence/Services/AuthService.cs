using Finance.Application.Abstractions;
using Finance.Application.Commands;
using Finance.Application.Exceptions;
using Finance.Application.Responses;
using Finance.Infrastructure.Common;
using Finance.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System.Security.Authentication;

namespace Finance.Infrastructure.Persistence.Services;

public class AuthService(UserManager<ApplicationUser> userManager, IJwtService jwtService, ICategoryRepository categoryRepository) : IAuthService
{
    private readonly UserManager<ApplicationUser> userManager = userManager;

    private readonly IJwtService jwtService = jwtService;

    private readonly ICategoryRepository categoryRepository = categoryRepository;

    public async Task<AuthResponse> LoginAsync(LoginCommand request)
    {
        ApplicationUser user = await userManager.FindByEmailAsync(request.Email) ?? throw new EntityNotFoundException("User", request.Email);
        if (!await userManager.CheckPasswordAsync(user, request.Password))
        {
            throw new AuthenticationException("Invalid email or password.");
        }
        AuthResponse authResponse = new(await jwtService.GenerateTokenAsync(user.Id, request.Email));
        return authResponse;
    }

    public async Task<AuthResponse> RegisterAsync(RegistrationCommand request)
    {
        ApplicationUser user = new(request.FirstName, request.LastName, request.DateOfBirth, request.Email);
        IdentityResult result = await userManager.CreateAsync(user, request.Password);
        result.ThrowIfHasError();
        AuthResponse authResponse = new(await jwtService.GenerateTokenAsync(user.Id, request.Email));
        await CreateCategoriesAsync(user.Id);
        return authResponse;
    }

    private async Task CreateCategoriesAsync(string userId)
    {
        IEnumerable<string> names = ["Food", "Restaurant", "Business", "Work"];
        foreach (string name in names)
            await categoryRepository.CreateAsync(new(name, userId));
    }
}
