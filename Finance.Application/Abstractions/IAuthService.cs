using Finance.Application.Commands;
using Finance.Application.Responses;

namespace Finance.Application.Abstractions;

public interface IAuthService
{
    Task<AuthResponse> LoginAsync(LoginCommand request);
    Task<AuthResponse> RegisterAsync(RegistrationCommand request);
}
