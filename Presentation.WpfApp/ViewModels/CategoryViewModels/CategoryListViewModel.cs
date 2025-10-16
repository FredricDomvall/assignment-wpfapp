using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Presentation.WpfApp.ViewModels.ManufacturerViewModels;
using Presentation.WpfApp.ViewModels.ProductViewModels;
using System.Collections.ObjectModel;
using System.Windows;

namespace Presentation.WpfApp.ViewModels.CategoryViewModels;
public partial class CategoryListViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IJsonFileRepository<Category> _categoryRepository;
    private readonly ICategoryService _categoryService;

    public CategoryListViewModel(IServiceProvider serviceProvider, IJsonFileRepository<Category> categoryFileRepository, ICategoryService categoryService)
    {
        _serviceProvider = serviceProvider;
        _categoryService = categoryService;
        _categoryRepository = categoryFileRepository;
        LoadCategories();
    }

    [ObservableProperty]
    private string _title = "Category List";

    [ObservableProperty]
    private Category? _currentCategoryDetails;
    [ObservableProperty]
    private Visibility? _detailsVisibility = Visibility.Collapsed;
    [ObservableProperty]
    private Visibility? _detailsVisibilityInfo = Visibility.Visible;

    [RelayCommand]
    private void ShowCategoryDetails(Category category)
    {
        CurrentCategoryDetails = category;
        DetailsVisibility = category != null ? Visibility.Visible : Visibility.Collapsed;
        DetailsVisibilityInfo = Visibility.Collapsed;
    }

    [ObservableProperty]
    private ObservableCollection<Category> _categoryList = new();

    private void LoadCategories()
    {
        var loadResult = _categoryService.GetAllCategoriesFromList();
        if (loadResult.Statement is true)
            CategoryList = new ObservableCollection<Category>(loadResult.Outcome!);
        else
            CategoryList = new ObservableCollection<Category>();
    }
    [RelayCommand]
    private async Task DeleteCategory(Category category)
    {
        if (category is null) return;
        var deleteResult = await _categoryService.DeleteCategoryFromListByIdAsync(category.CategoryId);
        if (deleteResult.Statement is true)
            LoadCategories();
    }

    /***********************************************************************************
     *                          ONLY NAVIGATION COMMANDS BELOW                         *
     ***********************************************************************************/
    [RelayCommand]
    private void NavigateToCategoryCreateView()
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
    private void NavigateToCategoryUpdateView(Category category)
    {
        ShowCategoryDetails(category);
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        var categoryUpdateViewModel = _serviceProvider.GetRequiredService<CategoryUpdateViewModel>();
        categoryUpdateViewModel.CurrentCategoryDetails = CurrentCategoryDetails;
        mainViewModel.CurrentViewModel = categoryUpdateViewModel;
    }

    [RelayCommand]
    private void NavigateToProductListView()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        var productListViewModel = _serviceProvider.GetRequiredService<ProductListViewModel>();
        mainViewModel.CurrentViewModel = productListViewModel;
    }

    [RelayCommand]
    private void NavigateToManufacturerListView()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        var manufacturerListViewModel = _serviceProvider.GetRequiredService<ManufacturerListViewModel>();
        mainViewModel.CurrentViewModel = manufacturerListViewModel;
    }
}