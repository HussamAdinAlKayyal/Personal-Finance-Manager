using Finance.Domain.Entities;

namespace Finance.Test.Domain;

public static class CategoryTests
{
    private readonly static Category category = new("a", "");

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public static void SetName_EitherNullOrEmptyOrWhiteSpaceName_ShouldFail(string name)
    {
        Assert.Throws<ArgumentException>(() => category.SetName(name));
    }

    [Theory]
    [InlineData("a")]
    [InlineData("hello")]
    [InlineData("Hussam Al-Din Al-Kayyal")]
    public static void SetName_NeitherNullNorEmptyNorWhiteSpaceName_ShouldSucceed(string name)
    {
        category.SetName(name);
        Assert.Equal(name, category.Name);
    }
}
