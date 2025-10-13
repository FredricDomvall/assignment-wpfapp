namespace Infrastructure.Helpers;
public static class GeneratorHelper
{
    public static Guid GenerateGuidId() => Guid.NewGuid();
    public static string GenerateCategoryPrefix(string categoryName)
    {
        return categoryName.Trim().ToUpper().Substring(0,3);
    }
    public static string GenerateArticleNumber(string prefix)
    {
        var random = new Random();
        var randomDigits = random.Next(0, 99999);
        return $"{prefix}-{randomDigits.ToString("D5")}";
    }
}
