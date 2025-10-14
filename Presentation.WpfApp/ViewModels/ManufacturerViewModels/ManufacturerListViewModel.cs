using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Presentation.WpfApp.ViewModels.CategoryViewModels;
using Presentation.WpfApp.ViewModels.ProductViewModels;

namespace Presentation.WpfApp.ViewModels.ManufacturerViewModels;
public partial class ManufacturerListViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    public ManufacturerListViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    [ObservableProperty]
    private string _title = "Manufacturer List";

    [RelayCommand]
    private void NavigateToManufacturerUpdateView()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        var manufacturerListViewModel = _serviceProvider.GetRequiredService<ManufacturerListViewModel>();
        mainViewModel.CurrentViewModel = manufacturerListViewModel;
    }
    [RelayCommand]
    private void NavigateToManufacturerCreateView()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        var manufacturerCreateViewModel = _serviceProvider.GetRequiredService<ManufacturerCreateViewModel>();
        mainViewModel.CurrentViewModel = manufacturerCreateViewModel;
    }
    [RelayCommand]
    private void NavigateToCategoryListView()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        var categoryListViewModel = _serviceProvider.GetRequiredService<CategoryListViewModel>();
        mainViewModel.CurrentViewModel = categoryListViewModel;
    }
    [RelayCommand]
    private void NavigateToProductListView()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        var productListViewModel = _serviceProvider.GetRequiredService<ProductListViewModel>();
        mainViewModel.CurrentViewModel = productListViewModel;
    }

}
