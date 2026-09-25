namespace Finance.Domain.ValueObjects;

public sealed class Money
{
    public decimal Amount { get; private set; }
    public int CurrencyId { get; set; }

    public Money(decimal amount, int currencyId)
    {
        SetAmount(amount);
        CurrencyId = currencyId;
    }

    public void SetAmount(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Amount cannot be negative or zero.", nameof(amount));
        }
        Amount = amount;
    }
}
