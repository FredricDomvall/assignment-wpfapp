using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.Extensions.DependencyInjection;
using Presentation.WpfApp.ViewModels.CategoryViewModels;
using Presentation.WpfApp.ViewModels.ManufacturerViewModels;
using Presentation.WpfApp.ViewModels.ProductViewModels;
using System.Collections.ObjectModel;

namespace Presentation.WpfApp.ViewModels;

public partial class StartViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;
    private readonly IManufacturerService _manufacturerService;
    public StartViewModel(
        IServiceProvider serviceProvider, 
        IProductService productService, 
        ICategoryService categoryService, 
        IManufacturerService manufacturerService)
    {
        _serviceProvider = serviceProvider;
        _productService = productService;
        _categoryService = categoryService;
        _manufacturerService = manufacturerService;
        LoadProducts();

    }
    [ObservableProperty]
    private string _title = "Start View Model";
    [ObservableProperty]
    private ObservableCollection<Product> _productList = new();
    [ObservableProperty]
    private ObservableCollection<Category> _categoryList = new();
    [ObservableProperty]
    private ObservableCollection<Manufacturer> _manufacturerList = new();
    private void LoadProducts()
    {
        var loadResult = _productService.GetAllProductsFromList();
        if (loadResult.Statement is true)
            ProductList = new ObservableCollection<Product>(loadResult.Outcome!);
        else
            ProductList = new ObservableCollection<Product>();
        var loadCategoriesResult = _categoryService.GetAllCategoriesFromList();
        if (loadCategoriesResult.Statement is true)
            CategoryList = new ObservableCollection<Category>(loadCategoriesResult.Outcome!);
        else
            CategoryList = new ObservableCollection<Category>();
        var loadManufacturersResult = _manufacturerService.GetAllManufacturersFromList();
        if (loadManufacturersResult.Statement is true)
            ManufacturerList = new ObservableCollection<Manufacturer>(loadManufacturersResult.Outcome!);
        else
            ManufacturerList = new ObservableCollection<Manufacturer>();
    }

    [RelayCommand]
    private void NavigateToProductListView()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        var productListViewModel = _serviceProvider.GetRequiredService<ProductListViewModel>();
        mainViewModel.CurrentViewModel = productListViewModel;
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
