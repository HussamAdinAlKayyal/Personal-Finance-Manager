using Finance.Api.Requests;
using Finance.Application.Abstractions;
using Finance.Application.Responses;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Finance.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(
    IAuthService authService,
    IValidator<RegistrationRequest> registrationRequestValidator,
    IValidator<LoginRequest> loginRequestValidator) : ControllerBase
{
    private readonly IAuthService authService = authService;

    private readonly IValidator<RegistrationRequest> registrationRequestValidator = registrationRequestValidator;

    private readonly IValidator<LoginRequest> loginRequestValidator = loginRequestValidator;

    [HttpPost("register")]
    public async Task<AuthResponse> RegisterAsync(RegistrationRequest request)
    {
        await registrationRequestValidator.ValidateAndThrowAsync(request);
        return await authService.RegisterAsync(new(request.FirstName, request.LastName, request.DateOfBirth, request.Email, request.Password));
    }

    [HttpPost("login")]
    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        await loginRequestValidator.ValidateAndThrowAsync(request);
        return await authService.LoginAsync(new(request.Email, request.Password));
    }
}
