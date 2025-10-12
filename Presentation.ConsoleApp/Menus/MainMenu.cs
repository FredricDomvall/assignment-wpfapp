namespace Presentation.ConsoleApp.Menus;
internal class MainMenu
{
    private readonly ProductMenu _productMenu;
    public MainMenu(ProductMenu productMenu)
    {
        _productMenu = productMenu;
    }
    public async Task Run()
    {
        await DisplayMainMenu();
    }
    private async Task DisplayMainMenu()
    {
        Console.WriteLine("=== MAIN MENU ===");
        Console.WriteLine("1. Product Menu");
        Console.WriteLine("0. Exit");
        Console.Write("Select an option: ");
        var option = Console.ReadLine();
        switch (option)
        {
            case "1":
                await DisplayProductMenu();
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
    private void ExitApplication()
    {
        Environment.Exit(0);
    }
}
