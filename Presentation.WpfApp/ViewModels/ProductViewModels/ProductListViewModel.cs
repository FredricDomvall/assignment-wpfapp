using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.Extensions.DependencyInjection;
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
        _ = LoadProductsAsync();
    }
    [ObservableProperty]
    private string _title = "Product List";

    [ObservableProperty]
    private Product? _currentProductDetails;
    [RelayCommand]
    private void ShowProductDetails(Product product)
    {
        CurrentProductDetails = product;
    }

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
    private async Task DeleteProduct(Product product)
    {
        if (product is null) return;
        var deleteResult = await _productService.DeleteProductFromListByIdAsync(product.ProductId);
        if (deleteResult.Statement is true)
            await LoadProductsAsync();
    }
    /***********************************************************************************
     *                          ONLY NAVIGATION COMMANDS BELOW                         *
     ***********************************************************************************/
    [RelayCommand]
    private void NavigateToProductCreateView()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        var productCreateViewModel = _serviceProvider.GetRequiredService<ProductCreateViewModel>();
        mainViewModel.CurrentViewModel = productCreateViewModel;
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
        var productUpdateViewModel = _serviceProvider.GetRequiredService<ProductUpdateViewModel>();
        productUpdateViewModel.CurrentProductDetails = CurrentProductDetails;
        mainViewModel.CurrentViewModel = productUpdateViewModel;
    }
}