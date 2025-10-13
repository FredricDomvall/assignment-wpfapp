using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Presentation.ConsoleApp.Menus;
internal class ProductMenu
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;
    private readonly IManufacturerService _manufacturerService;
    private readonly ManufacturerMenu ManufacturerMenu;
    private readonly CategoryMenu _categoryMenu;
    public ProductMenu(
        IProductService productService, 
        ICategoryService categoryService,
        IManufacturerService manufacturerService
,
        ManufacturerMenu manufacturerMenu,
        CategoryMenu categoryMenu)
    {
        _productService = productService;
        _categoryService = categoryService;
        _manufacturerService = manufacturerService;

        ManufacturerMenu = manufacturerMenu;
        _categoryMenu = categoryMenu;
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
            Console.WriteLine($"Manufacturer: {product.Manufacturer.ManufacturerName}\t {product.Manufacturer.ManufacturerCountry}\t {product.Manufacturer.ManufacturerEmail}");
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
            var categoryChoice = await ChooseCategoryForNewProduct();
            var manufacturerChoice = await ChooseManufacturerForNewProduct();
            if (manufacturerChoice.ManufacturerName == "N/A")
            {
                Console.WriteLine("No manufacturer selected. Product will be added without a manufacturer.");
            }
            else
            {
                Console.WriteLine($"Selected Manufacturer: {manufacturerChoice.ManufacturerName}");
            }
            var productForm = new ProductForm
            {
                ProductName = productName,
                ProductPrice = productPrice,
                CategoryName = categoryChoice,
            };
            var addProductResult = await _productService.AddProductToListAsync(productForm);
            Console.WriteLine(addProductResult.Answer);

            Console.WriteLine("Press '1' to add another product or any other key to return to the menu.");
        } while (Console.ReadLine() == "1");
    }

    private async Task<string> ChooseCategoryForNewProduct()
    {
        var categoryResult = await _categoryService.GetAllCategoriesFromListAsync();
        while (!categoryResult.Statement || categoryResult.Outcome is null || !categoryResult.Outcome.Any())
        {
            Console.WriteLine(categoryResult.Answer);
            Console.WriteLine("No categories available. Please add a category first.");
            Console.WriteLine("Press any key to add a new category...");
            Console.ReadKey();
            Category newCategory = new();
            await _categoryMenu.AddNewCategoryToList();
            categoryResult = await _categoryService.GetAllCategoriesFromListAsync();
        }

        var categories = categoryResult.Outcome!.ToList();
        Console.WriteLine("Available Categories:");
        for (int i = 0; i < categories.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {categories[i].CategoryName} (Prefix: {categories[i].CategoryPrefix})");
        }
        Console.Write("Select a category by number or press any other key if not available: ");
        if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= categories.Count)
        {
            return categories[choice - 1].CategoryName;
        }
        else
        {
            return "N/A";
        }
    }
    private async Task<Manufacturer> ChooseManufacturerForNewProduct()
    {
        var manufacturerResult = await _manufacturerService.GetAllManufacturersFromListAsync();
        while (!manufacturerResult.Statement || manufacturerResult.Outcome is null || !manufacturerResult.Outcome.Any())
        {
            Console.WriteLine(manufacturerResult.Answer);
            Console.WriteLine("Press any key to add a new manufacturer...");
            Console.ReadKey();
            Manufacturer newManufacturer = new();
            await ManufacturerMenu.AddNewManufacturerToList();
            manufacturerResult = await _manufacturerService.GetAllManufacturersFromListAsync();
        }

        var manufacturers = manufacturerResult.Outcome!.ToList();
        Console.WriteLine("Available Manufacturers:");
        for (int i = 0; i < manufacturers.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {manufacturers[i].ManufacturerName} (Country: {manufacturers[i].ManufacturerCountry})");
        }
        Console.Write("Select a manufacturer by number or press any other key if not availible: ");
        if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= manufacturers.Count)
        {
            return manufacturers[choice - 1];
        }
        else
        {
            Manufacturer noManufacturer = new() { ManufacturerName = "N/A", ManufacturerCountry = "N/A", ManufacturerEmail = "N/A" };
            return noManufacturer;
        }
    }
}
