namespace Finance.Domain.Entities;

public class Currency
{
    public int Id { get; set; }
    public string Name { get; private set; } = null!;
    public string Code { get; private set; } = null!;

    public Currency(string name, string code)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Currency name cannot be null or empty.", nameof(name));
        }
        if (string.IsNullOrWhiteSpace(code) || code.Length != 3)
        {
            throw new ArgumentException("Currency code must be a 3-letter uppercase string.", nameof(code));
        }
        Name = name;
        Code = code.ToUpper();
    }
}
