using CommunityToolkit.Mvvm.ComponentModel;

namespace Presentation.WpfApp.ViewModels.ProductViewModels;
public partial class ProductCreateViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    public ProductCreateViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    [ObservableProperty]
    private string _title = "Create New Product";
}
