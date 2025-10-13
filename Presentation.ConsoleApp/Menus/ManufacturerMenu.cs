using Infrastructure.Interfaces;

namespace Presentation.ConsoleApp.Menus;
public class ManufacturerMenu
{
    private readonly IManufacturerService _manufacturerService;
    public ManufacturerMenu(IManufacturerService manufacturerService)
    {
        _manufacturerService = manufacturerService;
    }

}
