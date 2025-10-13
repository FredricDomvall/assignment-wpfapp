using Infrastructure.Interfaces;

namespace Presentation.ConsoleApp.Menus;
public class ManufacturerMenu
{
    private readonly IManufacturerService _manufacturerService;
    public ManufacturerMenu(IManufacturerService manufacturerService)
    {
        _manufacturerService = manufacturerService;
    }
    public async Task Run()
    {
        await DisplayManufacturerMenu();
    }
    private async Task DisplayManufacturerMenu()
    {
        Console.Clear();
        Console.WriteLine("=== MANUFACTURER MENU ===");
        Console.WriteLine("1. List Manufacturers");
        Console.WriteLine("2. Add Manufacturer");
        Console.WriteLine("0. Back to Main Menu");
        Console.Write("Select an option: ");
        var option = Console.ReadLine();
        switch (option)
        {
            case "1":
                //await ShowManufacturersInList();
                break;
            case "2":
                //await AddNewManufacturerToList();
                break;
            case "0":
                return;
            default:
                Console.WriteLine("Invalid choice. Please try again.");
                break;
        }
        await DisplayManufacturerMenu();
    }

}
