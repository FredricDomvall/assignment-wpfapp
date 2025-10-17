using Infrastructure.Models;

namespace Infrastructure.Helpers;
public static class GeneratorHelper
{
    public static Guid GenerateGuidId() => Guid.NewGuid();
    public static string GenerateCategoryPrefix(string categoryName)
    {
        if (string.IsNullOrWhiteSpace(categoryName))
            return string.Empty;

        if (categoryName.Length < 3)
            return string.Empty;

        return categoryName.Trim().ToUpper().Substring(0, 3);
    }
    public static string GenerateArticleNumber(string prefix, List<Product> productList)
    {
        int index = 1;
        string newArticleNumber;

        do
        {
            newArticleNumber = $"{prefix}-{index:D5}";
            index++;
        }
        while (productList.Any(p => p.ProductCode == newArticleNumber));

        return newArticleNumber;
    }
}
