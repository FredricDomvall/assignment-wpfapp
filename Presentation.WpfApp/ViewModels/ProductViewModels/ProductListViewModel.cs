using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace Presentation.WpfApp.ViewModels.ProductViewModels;
public partial class ProductListViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    public ProductListViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    [ObservableProperty]
    private string _title = "Product List";

    [RelayCommand]
    private void NavigateToProductCreateView()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        var productCreateViewModel = _serviceProvider.GetRequiredService<ProductCreateViewModel>();
        mainViewModel.CurrentViewModel = productCreateViewModel;
    }
}