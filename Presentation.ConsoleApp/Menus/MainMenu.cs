namespace Presentation.ConsoleApp.Menus;
internal class MainMenu
{
    public void Run()
    {
        DisplayMainMenu();
    }
    private void DisplayMainMenu()
    {
        Console.WriteLine("Main Menu");
        Console.WriteLine("1. Product Menu");
        Console.WriteLine("0. Exit");
        Console.Write("Select an option: ");
        var option = Console.ReadLine();
        switch (option)
        {
            case "1":
                var productMenu = new ProductMenu();
                //productMenu.DisplayProductMenu();
                break;
            case "0":
                ExitApplication();
                break;
            default:
                Console.WriteLine("Invalid choice. Please try again.");
                break;
        }
        DisplayMainMenu();
    }
    private void ExitApplication()
    {
        Environment.Exit(0);
    }
}
