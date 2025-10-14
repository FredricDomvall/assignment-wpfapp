using CommunityToolkit.Mvvm.ComponentModel;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Presentation.WpfApp.ViewModels.ProductViewModels;
public partial class ProductCreateViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IJsonFileRepository<Product> _productRepository;
    private readonly IProductService _productService;
    public ProductCreateViewModel(IServiceProvider serviceProvider, IJsonFileRepository<Product> productRepository, IProductService productService)
    {
        _serviceProvider = serviceProvider;
        _productRepository = productRepository;
        _productService = productService;
    }
    [ObservableProperty]
    private string _title = "Create New Product";
    
    private void SaveNewProduct()
    {
    }
    private void CancelNewProductCreation()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        var productListViewModel = _serviceProvider.GetRequiredService<ProductListViewModel>();
        mainViewModel.CurrentViewModel = productListViewModel;
    }
}
