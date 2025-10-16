using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace Presentation.WpfApp.ViewModels.CategoryViewModels;
public partial class CategoryUpdateViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ICategoryService _categoryService;
    public CategoryUpdateViewModel(IServiceProvider serviceProvider, ICategoryService categoryService)
    {
        _serviceProvider = serviceProvider;
        _categoryService = categoryService;

    }

    [ObservableProperty]
    private string _title = "Update Category";

    [ObservableProperty]
    public Category? _currentCategoryDetails;

    [RelayCommand]
    private async Task UpdateCategory()
    {
        if (CurrentCategoryDetails is null)
        {
            MessageBox.Show("All Fields Required. Try again!");
            return;
        }

        var categoryToUpdate = new Category
        {
            CategoryId = CurrentCategoryDetails.CategoryId,
            CategoryName = CurrentCategoryDetails.CategoryName,
            CategoryPrefix = CurrentCategoryDetails.CategoryPrefix
        };

        var updateResult = await _categoryService.UpdateCategoryInListByIdAsync(CurrentCategoryDetails.CategoryId, categoryToUpdate);
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        var categoryListViewModel = _serviceProvider.GetRequiredService<CategoryListViewModel>();
        mainViewModel.CurrentViewModel = categoryListViewModel;
    }
    [RelayCommand]
    private void NavigateToCategoryListView()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        var categoryListViewModel = _serviceProvider.GetRequiredService<CategoryListViewModel>();
        mainViewModel.CurrentViewModel = categoryListViewModel;
    }
}
