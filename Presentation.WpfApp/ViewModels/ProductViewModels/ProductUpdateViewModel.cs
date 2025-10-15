using CommunityToolkit.Mvvm.ComponentModel;
using Infrastructure.Models;

namespace Presentation.WpfApp.ViewModels.ProductViewModels;
public partial class ProductUpdateViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    public ProductUpdateViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    [ObservableProperty]
    private string _title = "Update Product";

    public Product? CurrentProductDetails { get; internal set; }
}
