using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.Extensions.DependencyInjection;
using Presentation.WpfApp.ViewModels.CategoryViewModels;
using Presentation.WpfApp.ViewModels.ManufacturerViewModels;
using System.Collections.ObjectModel;

namespace Presentation.WpfApp.ViewModels.ProductViewModels;
public partial class ProductListViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IJsonFileRepository<Product> _productRepository;
    private readonly IProductService _productService;
 
    public ProductListViewModel(IServiceProvider serviceProvider, IJsonFileRepository<Product> productFileRepository, IProductService productService)
    {
        _serviceProvider = serviceProvider;
        _productService = productService;
        _productRepository = productFileRepository;
    }
    [ObservableProperty]
    private string _title = "Product List";

    [ObservableProperty]
    private ObservableCollection<Product> _productList = new();

    private async Task LoadProductsAsync()
    {
        var loadResult = await _productService.GetAllProductsFromListAsync();
        if (loadResult.Statement is true)
            ProductList = new ObservableCollection<Product>(loadResult.Outcome!);
        else
            ProductList = new ObservableCollection<Product>();
    }
    [RelayCommand]
    private async Task RefreshProductList()
    {
        await LoadProductsAsync();
    }
    /***********************************************************************************
     *                          ONLY NAVIGATION COMMANDS BELOW                         *
     ***********************************************************************************/
    [RelayCommand]
    private void NavigateToProductCreateView()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        var categoryCreateViewModel = _serviceProvider.GetRequiredService<CategoryCreateViewModel>();
        mainViewModel.CurrentViewModel = categoryCreateViewModel;
    }
    [RelayCommand]
    private void NavigateToStartView()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        var startViewModel = _serviceProvider.GetRequiredService<StartViewModel>();
        mainViewModel.CurrentViewModel = startViewModel;
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