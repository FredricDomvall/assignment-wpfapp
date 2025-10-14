using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace Presentation.WpfApp.ViewModels.ProductViewModels;
public partial class ProductCreateViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IJsonFileRepository<Product> _productRepository;
    private readonly IProductService _productService;
    private readonly IManufacturerService _manufacturerService;
    private readonly ICategoryService _categoryService;
    public ProductCreateViewModel(
        IServiceProvider serviceProvider, 
        IJsonFileRepository<Product> productRepository, 
        IProductService productService, 
        IManufacturerService manufacturerService, 
        ICategoryService categoryService)
    {
        _serviceProvider = serviceProvider;
        _productRepository = productRepository;
        _productService = productService;
        _manufacturerService = manufacturerService;
        _categoryService = categoryService;
        
    }
    [ObservableProperty]
    private string _title = "Create New Product";
    [ObservableProperty]
    private ObservableCollection<Category> _categories = new();
    [ObservableProperty]
    private ObservableCollection<Manufacturer> _manufacturers = new();
    [ObservableProperty]
    private ProductForm? _newProduct = new ProductForm();

    private async Task LoadOtherListsAsync()
    {
        var loadCategoriesResult = await _categoryService.GetAllCategoriesFromListAsync();
        if (loadCategoriesResult.Statement is true)
            Categories = new ObservableCollection<Category>(loadCategoriesResult.Outcome!);
        else
            Categories = new ObservableCollection<Category>();

        var loadManufacturersResult = await _manufacturerService.GetAllManufacturersFromListAsync();
        if (loadManufacturersResult.Statement is true)
            Manufacturers = new ObservableCollection<Manufacturer>(loadManufacturersResult.Outcome!);
        else
            Manufacturers = new ObservableCollection<Manufacturer>();
    }
    [RelayCommand]
    private async Task RefreshOtherLists()
    {
        await LoadOtherListsAsync();
    }


    [RelayCommand]
    private async Task SaveNewProduct()
    {
        if (NewProduct is null) return;
        var addResult = await _productService.AddProductToListAsync(NewProduct);
        if (addResult.Statement is true)
        {
            var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
            var productListViewModel = _serviceProvider.GetRequiredService<ProductListViewModel>();
            mainViewModel.CurrentViewModel = productListViewModel;
        }

    }
    [RelayCommand]
    private void CancelNewProductCreation()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        var productListViewModel = _serviceProvider.GetRequiredService<ProductListViewModel>();
        mainViewModel.CurrentViewModel = productListViewModel;
    }

    /***********************************************************************************
     *                          ONLY NAVIGATION METHODS BELOW                          *
     ***********************************************************************************/

}
