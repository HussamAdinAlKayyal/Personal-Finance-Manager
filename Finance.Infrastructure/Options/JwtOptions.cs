namespace Finance.Infrastructure.Options;

public class JwtOptions
{
    public const string Section = "Jwt";
    public string Audience { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string Key { get; set; } = null!;
    public double ExpirationMinutes { get; set; }
    public int ExpirationMonths { get; set; }
}
