using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Xml;

namespace Presentation.WpfApp.ViewModels.ProductViewModels;
public partial class ProductUpdateViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IProductService _productService;
    public ProductUpdateViewModel(IServiceProvider serviceProvider, IProductService productService)
    {
        _serviceProvider = serviceProvider;
        _productService = productService;
    }
    [ObservableProperty]
    private string _title = "Update Product";
    [ObservableProperty]
    public Product? _currentProductDetails;

    [RelayCommand]
    private async Task UpdateProduct()
    {
        ProductForm productToUpdate = new ProductForm();
        if (CurrentProductDetails is null)
        {
            MessageBox.Show("All Fields Required. Try again!");
            return;
        }
        productToUpdate.ProductName = CurrentProductDetails.ProductName;
        productToUpdate.ProductPrice = CurrentProductDetails.ProductPrice.ToString();
        productToUpdate.ProductCode = CurrentProductDetails.ProductCode;
        productToUpdate.ProductDescription = CurrentProductDetails.ProductDescription;
        productToUpdate.CategoryName = CurrentProductDetails.Category.CategoryName;
        productToUpdate.ManufacturerName = CurrentProductDetails.Manufacturer.ManufacturerName;
        productToUpdate.ManufacturerEmail = CurrentProductDetails.Manufacturer.ManufacturerEmail;
        productToUpdate.ManufacturerCountry = CurrentProductDetails.Manufacturer.ManufacturerCountry;

        var updateResult = await _productService.UpdateProductInListByIdAsync(CurrentProductDetails.ProductId, productToUpdate);
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        var productListViewModel = _serviceProvider.GetRequiredService<ProductListViewModel>();
        mainViewModel.CurrentViewModel = productListViewModel;
    }
}
