using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Presentation.ConsoleApp.Menus;
internal class CategoryMenu
{
    private readonly ICategoryService _categoryService;
    public CategoryMenu(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }
    public async Task Run()
    {
        await DisplayCategoryMenu();
    }
    private async Task DisplayCategoryMenu()
    {
        Console.Clear();

        Console.WriteLine("=== CATEGORY MENU ===");
        Console.WriteLine("1. List Categories");
        Console.WriteLine("2. Add Category");
        Console.WriteLine("0. Back to Main Menu");
        Console.Write("Select an option: ");
        var option = Console.ReadLine();
        switch (option)
        {
            case "1":
                await ShowCategoriesInList();
                break;
            case "2":
                await AddNewCategoryToList();
                break;
            case "0":
                return;
            default:
                Console.WriteLine("Invalid choice. Please try again.");
                break;
        }
        await DisplayCategoryMenu();
    }
    private async Task ShowCategoriesInList()
    {
        Console.Clear();
        Console.WriteLine("----------  CATEGORY LIST  ----------");
        var categoryResult = await _categoryService.GetAllCategoriesFromListAsync();
        if (!categoryResult.Statement || categoryResult.Outcome is null)
        {
            Console.WriteLine("No categories available.");
            Console.WriteLine("Press any key to return to the menu...");
            Console.ReadKey();
            return;
        }
        foreach (var category in categoryResult.Outcome)
        {
            Console.WriteLine($"ID: {category.CategoryId}");
            Console.WriteLine($"Name: {category.CategoryName} Prefix: {category.CategoryPrefix}");
            Console.WriteLine("--------------------------------");
        }
        Console.WriteLine("Press any key to return to the menu...");
        Console.ReadKey();
    }
    private async Task AddNewCategoryToList()
    {
        do
        {
            Console.Clear();
            Console.WriteLine("----------ADD NEW CATEGORY ---------- :");
            Console.WriteLine("");

            Console.Write("Enter Category Name: ");
            var categoryName = Console.ReadLine() ?? "";

            var categoryForm = new Category
            {
                CategoryName = categoryName
            };
            var addCategoryResult = await _categoryService.AddCategoryToListAsync(categoryForm);
            Console.WriteLine(addCategoryResult.Answer);

            Console.WriteLine("Press '1' to add another category or any other key to return to the menu.");
        } while (Console.ReadLine() == "1");
    }
}

