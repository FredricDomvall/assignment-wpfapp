using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Presentation.ConsoleApp.Menus;
internal class ProductMenu
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;
    private readonly IManufacturerService _manufacturerService;
    public ProductMenu(IProductService productService, ICategoryService categoryService, IManufacturerService manufacturerService)
    {
        _productService = productService;
        _categoryService = categoryService;
        _manufacturerService = manufacturerService;
    }
    public async Task Run()
    {
        await DisplayProductMenu();
    }
    private async Task DisplayProductMenu()
    {
        Console.Clear();

        Console.WriteLine("=== PRODUCT MENU ===");
        Console.WriteLine("1. List Products");
        Console.WriteLine("2. Add Product");
        Console.WriteLine("0. Back to Main Menu");
        Console.Write("Select an option: ");
        var option = Console.ReadLine();
        switch (option)
        {
            case "1":
                await ShowProductsInList();
                break;
            case "2":
                await AddNewProductToList();
                break;
            case "0":
                return;
            default:
                Console.WriteLine("Invalid choice. Please try again.");
                break;
        }
        await DisplayProductMenu();
    }
    private async Task ShowProductsInList()
    {
        Console.Clear();
        Console.WriteLine("----------  PRODUCT LIST  ----------");
        var productResult = await _productService.GetAllProductsFromListAsync();
        if (!productResult.Statement || productResult.Outcome is null || !productResult.Outcome.Any())
        {
            Console.WriteLine(productResult.Answer);
            Console.WriteLine("Press any key to return to the menu...");
            Console.ReadKey();
            return;
        }
        foreach (var product in productResult.Outcome)
        {
            Console.WriteLine($"ID: {product.ProductId}");
            Console.WriteLine($"Name: {product.ProductName}\t Price: {product.ProductPrice}");
            Console.WriteLine($"ProductCode: {product.ProductCode}\t Category: {product.Category.CategoryName}");
            Console.WriteLine("");
        }
        Console.WriteLine("Press any key to return to the menu...");
        Console.ReadKey();
    }
    private async Task AddNewProductToList()
    {
        do
        {
            Console.Clear();
            Console.WriteLine("----------ADD NEW PRODUCT ---------- :");
            Console.WriteLine("");

            Console.Write("Enter Product Name: ");
            var productName = Console.ReadLine();

            Console.Write("Enter Product Price: ");
            var productPrice = Console.ReadLine();          
            var result = await ChooseCategoryForNewProduct();
            var productForm = new ProductForm
            {
                ProductName = productName,
                ProductPrice = productPrice,
                CategoryName = result
            };
            var addProductResult = await _productService.AddProductToListAsync(productForm);
            Console.WriteLine(addProductResult.Answer);

            Console.WriteLine("Press '1' to add another product or any other key to return to the menu.");
        } while (Console.ReadLine() == "1");
    }

    private async Task<string> ChooseCategoryForNewProduct()
    {

        var categoryResult = await _categoryService.GetAllCategoriesFromListAsync();
        if (!categoryResult.Statement || categoryResult.Outcome is null || !categoryResult.Outcome.Any())
        {
            Console.WriteLine("No categories available. Please add a category first.");
            return "N/A";
        }
        var categories = categoryResult.Outcome.ToList();
        Console.WriteLine("Available Categories:");
        for (int i = 0; i < categories.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {categories[i].CategoryName} (Prefix: {categories[i].CategoryPrefix})");
        }
        Console.Write("Select a category by number or enter '0' to skip: ");
        if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= categories.Count)
        {
            return categories[choice - 1].CategoryName;
        }
        else
        {
            return "N/A";
        }

    }
}
