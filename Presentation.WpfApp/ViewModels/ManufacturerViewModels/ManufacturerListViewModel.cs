using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.Extensions.DependencyInjection;
using Presentation.WpfApp.ViewModels.CategoryViewModels;
using Presentation.WpfApp.ViewModels.ProductViewModels;
using System.Collections.ObjectModel;

namespace Presentation.WpfApp.ViewModels.ManufacturerViewModels;
public partial class ManufacturerListViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IJsonFileRepository<Manufacturer> _manufacturerRepository;
    private readonly IManufacturerService _manufacturerService;

    public ManufacturerListViewModel(IServiceProvider serviceProvider, IJsonFileRepository<Manufacturer> manufacturerFileRepository, IManufacturerService manufacturerService)
    {
        _serviceProvider = serviceProvider;
        _manufacturerService = manufacturerService;
        _manufacturerRepository = manufacturerFileRepository;
    }
    [ObservableProperty]
    private string _title = "Manufacturer List";

    [ObservableProperty]
    private Manufacturer? _currentManufacturerDetails;
    [RelayCommand]
    private void ShowManufacturerDetails(Manufacturer manufacturer)
    {
        CurrentManufacturerDetails = manufacturer;
    }

    [ObservableProperty]
    private ObservableCollection<Manufacturer> _manufacturerList = new();

    private void LoadManufacturers()
    {
        var loadResult = _manufacturerService.GetAllManufacturersFromList();
        if (loadResult.Statement is true)
            ManufacturerList = new ObservableCollection<Manufacturer>(loadResult.Outcome!);
        else
            ManufacturerList = new ObservableCollection<Manufacturer>();
    }
    [RelayCommand]
    private void NavigateToManufacturerCreateView()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        var manufacturerCreateViewModel = _serviceProvider.GetRequiredService<ManufacturerCreateViewModel>();
        mainViewModel.CurrentViewModel = manufacturerCreateViewModel;
    }
    [RelayCommand]
    private void NavigateToStartView()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        var startViewModel = _serviceProvider.GetRequiredService<StartViewModel>();
        mainViewModel.CurrentViewModel = startViewModel;
    }
    [RelayCommand]
    private void NavigateToManufacturerUpdateView()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        var manufacturerUpdateViewModel = _serviceProvider.GetRequiredService<ManufacturerUpdateViewModel>();
        mainViewModel.CurrentViewModel = manufacturerUpdateViewModel;
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
}