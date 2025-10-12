using Infrastructure.Services;

namespace Presentation.ConsoleApp.Menus;
internal class ProductMenu
{
    private readonly ProductService _productService;
    public ProductMenu(ProductService productService)
    {
        _productService = productService;
    }
    public void Run()
    {
        DisplayProductMenu();
    }
    private void DisplayProductMenu()
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
                ShowProductsInList();
                break;
            case "2":
                AddNewProductToList();
                break;
            case "0":
                return;
            default:
                Console.WriteLine("Invalid choice. Please try again.");
                break;
        }
        DisplayProductMenu();
    }
    private void ShowProductsInList()
    {
        Console.Clear();
        Console.WriteLine("----------  PRODUCT LIST  ----------");
        var products = _productService.GetAllProductsFromListAsync();
        if (!products.Outcome.Any())
        {
            Console.WriteLine("No products available.");
            Console.WriteLine("Press any key to return to the menu...");
            Console.ReadKey();
            return;
        }
        foreach (var product in products)
        {
            Console.WriteLine($"ID: {product.ProductId}");
            Console.WriteLine($"Name: {product.ProductName}, Price: {product.ProductPrice}");
            Console.WriteLine("--------------------------------");
        }
        Console.WriteLine("Press any key to return to the menu...");
        Console.ReadKey();
    }
    private void AddNewProductToList()
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

            var productForm = new Infrastructure.Models.ProductForm
            {
                ProductName = productName,
                ProductPrice = productPrice
            };
            var result = _productService.AddProductToListAsync(productForm);
            
            if (result)
                Console.WriteLine("Product added successfully.");
            else
                Console.WriteLine("Failed to add product. Please check the input values.");

            Console.WriteLine("Press '1' to add another product or any other key to return to the menu.");
        } while (Console.ReadLine() == "1");
    }
}
