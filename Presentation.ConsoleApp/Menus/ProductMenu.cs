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
        Console.WriteLine("Product Menu");
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
    }
    private void AddNewProductToList()
    {
    }
}
