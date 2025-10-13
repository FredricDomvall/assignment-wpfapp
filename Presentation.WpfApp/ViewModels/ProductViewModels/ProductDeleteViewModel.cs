using CommunityToolkit.Mvvm.ComponentModel;

namespace Presentation.WpfApp.ViewModels.ProductViewModels;
public partial class ProductDeleteViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    public ProductDeleteViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    [ObservableProperty]
    private string _title = "Delete Product From List";
}