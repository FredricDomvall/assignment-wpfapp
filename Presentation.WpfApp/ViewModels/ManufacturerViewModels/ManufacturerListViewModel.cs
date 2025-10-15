using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Infrastructure.Interfaces;
using Infrastructure.Models;

using Microsoft.Extensions.DependencyInjection;

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
        LoadManufacturers();
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
    private async Task DeleteManufacturer(Manufacturer manufacturer)
    {
        if (manufacturer is null) return;
        var deleteResult = await _manufacturerService.DeleteManufacturerFromListByIdAsync(manufacturer.ManufacturerId);
        if (deleteResult.Statement is true)
            LoadManufacturers();
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
    private void NavigateToManufacturerUpdateView(Manufacturer manufacturer)
    {
        ShowManufacturerDetails(manufacturer);
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        var manufacturerUpdateViewModel = _serviceProvider.GetRequiredService<ManufacturerUpdateViewModel>();
        manufacturerUpdateViewModel.CurrentManufacturerDetails = CurrentManufacturerDetails;
        mainViewModel.CurrentViewModel = manufacturerUpdateViewModel;
    }
}