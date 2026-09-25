using Microsoft.AspNetCore.Identity;

namespace Finance.Infrastructure.Common;

internal static class Helper
{
    public static void ThrowIfHasError(this IdentityResult result)
    {
        if (!result.Succeeded)
        {
            throw new ArgumentException(string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
}
