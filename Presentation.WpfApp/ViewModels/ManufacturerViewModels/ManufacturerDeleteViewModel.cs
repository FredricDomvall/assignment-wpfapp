using CommunityToolkit.Mvvm.ComponentModel;

namespace Presentation.WpfApp.ViewModels.ManufacturerViewModels;
public partial class ManufacturerDeleteViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    public ManufacturerDeleteViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    [ObservableProperty]
    private string _title = "Delete Manufacturer From List";
}
