namespace Presentation.ConsoleApp.Menus;
internal class MainMenu
{
    private readonly ProductMenu _productMenu;
    private readonly CategoryMenu _categoryMenu;
    public MainMenu(ProductMenu productMenu, CategoryMenu categoryMenu)
    {
        _productMenu = productMenu;
        _categoryMenu = categoryMenu;
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
    private void ExitApplication()
    {
        Environment.Exit(0);
    }
}
