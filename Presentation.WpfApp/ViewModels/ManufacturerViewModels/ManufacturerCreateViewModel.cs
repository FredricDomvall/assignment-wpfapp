using CommunityToolkit.Mvvm.ComponentModel;

namespace Presentation.WpfApp.ViewModels.ManufacturerViewModels;
public partial class ManufacturerCreateViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    public ManufacturerCreateViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    [ObservableProperty]
    private string _title = "Create New Manufacturer";
}
