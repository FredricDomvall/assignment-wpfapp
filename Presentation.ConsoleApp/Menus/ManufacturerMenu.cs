using Infrastructure.Interfaces;
using Infrastructure.Models;

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
                await ShowManufacturersInList();
                break;
            case "2":
                await AddNewManufacturerToList();
                break;
            case "0":
                return;
            default:
                Console.WriteLine("Invalid choice. Please try again.");
                break;
        }
        await DisplayManufacturerMenu();
    }

    private async Task ShowManufacturersInList()
    {
        Console.Clear();
        Console.WriteLine("----------  MANUFACTURER LIST  ----------");
        var manufacturerResult = await _manufacturerService.GetAllManufacturersFromListAsync();
        if (!manufacturerResult.Statement || manufacturerResult.Outcome is null)
        {
            Console.WriteLine(manufacturerResult.Answer);
            Console.WriteLine("Press any key to return to the menu...");
            Console.ReadKey();
            return;
        }
        foreach (var manufacturer in manufacturerResult.Outcome)
        {
            Console.WriteLine($"ID: {manufacturer.ManufacturerId}");
            Console.WriteLine($"Name: {manufacturer.ManufacturerName}\t Price: {manufacturer.ManufacturerCountry}\t Email: {manufacturer.ManufacturerEmail}");
        }
    }

    private async Task AddNewManufacturerToList()
    {
        do
        {
            Console.Clear();
            Console.WriteLine("----------  ADD NEW MANUFACTURER  ----------");
            Console.Write("Enter Manufacturer Name: ");
            var manufacturerName = Console.ReadLine() ?? "";
            Console.Write("Enter Manufacturer Country: ");
            var manufacturerCountry = Console.ReadLine() ?? "";
            Console.Write("Enter Manufacturer Email: ");
            var manufacturerEmail = Console.ReadLine() ?? "";

            Manufacturer manufacturerForm = new Manufacturer()
            {
                ManufacturerName = manufacturerName,
                ManufacturerCountry = manufacturerCountry,
                ManufacturerEmail = manufacturerEmail
            };
            var addManufacturerResult = await _manufacturerService.AddManufacturerToListAsync(manufacturerForm);
            Console.WriteLine(addManufacturerResult.Answer);

            Console.WriteLine("Press '1' to add another category or any other key to return to the menu.");
        } while (Console.ReadLine() == "1");
    }
}
