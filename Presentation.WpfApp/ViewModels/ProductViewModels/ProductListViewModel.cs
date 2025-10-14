using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Presentation.WpfApp.ViewModels.CategoryViewModels;
using Presentation.WpfApp.ViewModels.ManufacturerViewModels;

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
    private void NavigateToStartView()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        var startViewModel = _serviceProvider.GetRequiredService<StartViewModel>();
        mainViewModel.CurrentViewModel = startViewModel;
    }

    [RelayCommand]
    private void NavigateToProductCreateView()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        var categoryCreateViewModel = _serviceProvider.GetRequiredService<CategoryCreateViewModel>();
        mainViewModel.CurrentViewModel = categoryCreateViewModel;
    }
    [RelayCommand]
    private void NavigateToProductUpdateView()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        var categoryCreateViewModel = _serviceProvider.GetRequiredService<CategoryCreateViewModel>();
        mainViewModel.CurrentViewModel = categoryCreateViewModel;
    }
    [RelayCommand]
    private void NavigateToProductDeleteView()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        var categoryCreateViewModel = _serviceProvider.GetRequiredService<CategoryCreateViewModel>();
        mainViewModel.CurrentViewModel = categoryCreateViewModel;
    }

    [RelayCommand]
    private void NavigateToManufacturerListView()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        var manufacturerListViewModel = _serviceProvider.GetRequiredService<ManufacturerListViewModel>();
        mainViewModel.CurrentViewModel = manufacturerListViewModel;
    }
    [RelayCommand]
    private void NavigateToCategoryListView()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        var categoryListViewModel = _serviceProvider.GetRequiredService<CategoryListViewModel>();
        mainViewModel.CurrentViewModel = categoryListViewModel;
    }
}