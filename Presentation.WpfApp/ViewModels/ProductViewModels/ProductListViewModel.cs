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

    [RelayCommand]
    private void NavigateToCategoryListView()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        var categoryListViewModel = _serviceProvider.GetRequiredService<CategoryListViewModel>();
        mainViewModel.CurrentViewModel = categoryListViewModel;
    }
    [RelayCommand]
    private void NavigateToManufacturerListView()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        var manufacturerListViewModel = _serviceProvider.GetRequiredService<ManufacturerListViewModel>();
        mainViewModel.CurrentViewModel = manufacturerListViewModel;
    }
}