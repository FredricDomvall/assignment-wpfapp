namespace Presentation.ConsoleApp.Menus;
internal class MainMenu
{
    private readonly ProductMenu _productMenu;
    private readonly CategoryMenu _categoryMenu;
    private readonly ManufacturerMenu _manufacturerMenu;
    public MainMenu(ProductMenu productMenu, CategoryMenu categoryMenu, ManufacturerMenu manufacturerMenu)
    {
        _productMenu = productMenu;
        _categoryMenu = categoryMenu;
        _manufacturerMenu = manufacturerMenu;
    }
    public async Task Run()
    {
        await DisplayMainMenu();
    }
    private async Task DisplayMainMenu()
    {
        Console.Clear();
        Console.WriteLine("=== MAIN MENU ===");
        Console.WriteLine("1. Product Menu");
        Console.WriteLine("2. Category Menu");
        Console.WriteLine("3. Manufacturer Menu");
        Console.WriteLine("0. Exit");
        Console.Write("Select an option: ");
        var option = Console.ReadLine();
        switch (option)
        {
            case "1":
                await DisplayProductMenu();
                break;
            case "2":
                await DisplayCategoryMenu();
                break;
            case "3":
                await DisplayManufacturerMenu();
                break
            case "0":
                ExitApplication();
                break;
            default:
                Console.WriteLine("Invalid choice. Please try again.");
                break;
        }
        await DisplayMainMenu();
    }



    private async Task DisplayProductMenu()
    {
        await _productMenu.Run();
    }    
    private async Task DisplayCategoryMenu()
    {
        await _categoryMenu.Run();
    }
    private async Task DisplayManufacturerMenu()
    {
        await _manufacturerMenu.Run();
    }

    private void ExitApplication()
    {
        Environment.Exit(0);
    }
}
