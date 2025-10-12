using Infrastructure.Models;

namespace Infrastructure.Helpers
{
    public static class GeneratorHelper
    {
        public static Guid GenerateGuidId() => Guid.NewGuid();
        public static string GenerateCategoryPrefix(string categoryName)
        {
            return categoryName.Trim().ToUpper().Substring(0,3);
        }
    }
}
