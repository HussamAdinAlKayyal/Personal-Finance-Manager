namespace Finance.Domain.Entities;

public class Category
{
    public int Id { get; set; }
    public string Name { get; private set; } = null!;
    public string UserId { get; private set; }

    public Category(string name, string userId)
    {
        SetName(name);
        UserId = userId;
    }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Category name cannot be null or empty.", nameof(name));
        }
        Name = name;
    }
}
