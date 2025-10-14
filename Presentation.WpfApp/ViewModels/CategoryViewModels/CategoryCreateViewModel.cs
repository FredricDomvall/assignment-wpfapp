using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace Presentation.WpfApp.ViewModels.CategoryViewModels;
public partial class CategoryCreateViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IJsonFileRepository<Category> _categoryRepository;
    private readonly ICategoryService _categoryService;

    public CategoryCreateViewModel(
        IServiceProvider serviceProvider,
        IJsonFileRepository<Category> categoryRepository,
        ICategoryService categoryService)
    {
        _serviceProvider = serviceProvider;
        _categoryRepository = categoryRepository;
        _categoryService = categoryService;
    }

    [ObservableProperty]
    private string _title = "Create New Category";
    [ObservableProperty]
    private ObservableCollection<Category> _categories = new();
    [ObservableProperty]
    private Category? _newCategory = new Category();

    private async Task LoadCategoriesAsync()
    {
        var loadCategoriesResult = await _categoryService.GetAllCategoriesFromListAsync();
        if (loadCategoriesResult.Statement is true)
            Categories = new ObservableCollection<Category>(loadCategoriesResult.Outcome!);
        else
            Categories = new ObservableCollection<Category>();
    }

    [RelayCommand]
    private async Task RefreshCategories()
    {
        await LoadCategoriesAsync();
    }

    [RelayCommand]
    private async Task SaveNewCategory()
    {
        var addResult = await _categoryService.AddCategoryToListAsync(NewCategory!);
        if (addResult.Statement is true)
        {
            var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
            var categoryListViewModel = _serviceProvider.GetRequiredService<CategoryListViewModel>();
            mainViewModel.CurrentViewModel = categoryListViewModel;
        }
    }

    [RelayCommand]
    private void CancelNewCategoryCreation()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        var categoryListViewModel = _serviceProvider.GetRequiredService<CategoryListViewModel>();
        mainViewModel.CurrentViewModel = categoryListViewModel;
    }

    /***********************************************************************************
     *                          ONLY NAVIGATION METHODS BELOW                          *
     ***********************************************************************************/
}
