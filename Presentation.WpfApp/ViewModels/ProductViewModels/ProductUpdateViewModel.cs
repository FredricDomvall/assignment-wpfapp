using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows;

namespace Presentation.WpfApp.ViewModels.ProductViewModels;
public partial class ProductUpdateViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;
    private readonly IManufacturerService _manufacturerService;
    public ProductUpdateViewModel(
        IServiceProvider serviceProvider,
        IProductService productService,
        ICategoryService categoryService,
        IManufacturerService manufacturerService)
    {
        _serviceProvider = serviceProvider;
        _productService = productService;
        _categoryService = categoryService;
        _manufacturerService = manufacturerService;
        LoadOtherLists();
    }
    [ObservableProperty]
    private string _title = "Update Product";
    [ObservableProperty]
    public Product? _currentProductDetails;
    [ObservableProperty]
    private ObservableCollection<Category> _categories = new();
    [ObservableProperty]
    private Category? _selectedCategory;
    [ObservableProperty]
    private Manufacturer? _selectedManufacturer;
    [ObservableProperty]
    private ObservableCollection<Manufacturer> _manufacturers = new();

    private void LoadOtherLists()
    {
        var loadCategoriesResult = _categoryService.GetAllCategoriesFromList();
        if (loadCategoriesResult.Statement is true)
            Categories = new ObservableCollection<Category>(loadCategoriesResult.Outcome!);
        else
            Categories = new ObservableCollection<Category>();

        var loadManufacturersResult = _manufacturerService.GetAllManufacturersFromList();
        if (loadManufacturersResult.Statement is true)
            Manufacturers = new ObservableCollection<Manufacturer>(loadManufacturersResult.Outcome!);
        else
            Manufacturers = new ObservableCollection<Manufacturer>();
    }
    partial void OnSelectedCategoryChanged(Category? value)
    {
        if (CurrentProductDetails is not null && value is not null)
            CurrentProductDetails.Category = value;
    }


    [RelayCommand]
    private async Task UpdateProduct()
    {
        var updateResult = await _productService.UpdateProductInListByIdAsync(CurrentProductDetails!);
        if (updateResult.Statement is false)
            CurrentProductDetails = updateResult.Outcome;
        MessageBox.Show(updateResult.Answer);

        await _productService.LoadListFromFileAsync();
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        var productListViewModel = _serviceProvider.GetRequiredService<ProductListViewModel>();
        mainViewModel.CurrentViewModel = productListViewModel;
    }
    [RelayCommand]
    private void NavigateToProductListView()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        var productListViewModel = _serviceProvider.GetRequiredService<ProductListViewModel>();
        mainViewModel.CurrentViewModel = productListViewModel;
    }
}
