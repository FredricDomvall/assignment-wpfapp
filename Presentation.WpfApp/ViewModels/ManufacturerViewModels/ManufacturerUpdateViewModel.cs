using CommunityToolkit.Mvvm.ComponentModel;

namespace Presentation.WpfApp.ViewModels.ManufacturerViewModels;
public partial class ManufacturerUpdateViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    public ManufacturerUpdateViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    [ObservableProperty]
    private string _title = "Update Manufacturer";
}
