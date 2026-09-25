using Finance.Application.Abstractions;
using Finance.Application.Exceptions;
using Finance.Infrastructure.Identity;
using Finance.Infrastructure.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Finance.Infrastructure.Persistence.Services;

public class JwtService(UserManager<ApplicationUser> userManager, IOptions<JwtOptions> options) : IJwtService
{
    private readonly JwtOptions jwt = options.Value;

    private readonly UserManager<ApplicationUser> userManager = userManager;

    public async Task<string> GenerateTokenAsync(string userId, string email)
    {
        ApplicationUser user = await userManager.FindByIdAsync(userId) ?? 
            throw new EntityNotFoundException("User", userId);
        List<Claim> claims = [
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Actort, userId),
            new(JwtRegisteredClaimNames.NameId, userId),
            new(JwtRegisteredClaimNames.Email, email)];
        (await userManager.GetRolesAsync(user)).ToList().ForEach(s => claims.Add(new(ClaimTypes.Role, s)));
        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(jwt.Key));
        SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256);
        JwtSecurityToken token = new(
            issuer: jwt.Issuer,
            audience: jwt.Audience,
            claims: claims,
            notBefore: DateTime.Now,
            expires: DateTime.Now.AddMonths(jwt.ExpirationMonths),
            signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
