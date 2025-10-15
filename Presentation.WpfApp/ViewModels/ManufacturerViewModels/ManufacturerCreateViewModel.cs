using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace Presentation.WpfApp.ViewModels.ManufacturerViewModels;
public partial class ManufacturerCreateViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IJsonFileRepository<Manufacturer> _manufacturerRepository;
    private readonly IManufacturerService _manufacturerService;

    public ManufacturerCreateViewModel(
        IServiceProvider serviceProvider,
        IJsonFileRepository<Manufacturer> manufacturerRepository,
        IManufacturerService manufacturerService)
    {
        _serviceProvider = serviceProvider;
        _manufacturerRepository = manufacturerRepository;
        _manufacturerService = manufacturerService;
    }

    [ObservableProperty]
    private string _title = "Create New Manufacturer";
    [ObservableProperty]
    private Manufacturer? _newManufacturer = new Manufacturer();
    [ObservableProperty]
    private ObservableCollection<Manufacturer> _manufacturers = new();

    [RelayCommand]
    private async Task SaveNewManufacturer()
    {
        NewManufacturer!.ManufacturerName = NewManufacturer.ManufacturerName;
        NewManufacturer.ManufacturerCountry = NewManufacturer.ManufacturerCountry;
        NewManufacturer.ManufacturerEmail = NewManufacturer.ManufacturerEmail;
        var addResult = await _manufacturerService.AddManufacturerToListAsync(NewManufacturer);
        if (addResult.Statement is true)
        {
            var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
            var manufacturerListViewModel = _serviceProvider.GetRequiredService<ManufacturerListViewModel>();
            mainViewModel.CurrentViewModel = manufacturerListViewModel;
        }
    }

    [RelayCommand]
    private void CancelNewManufacturerCreation()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        var manufacturerListViewModel = _serviceProvider.GetRequiredService<ManufacturerListViewModel>();
        mainViewModel.CurrentViewModel = manufacturerListViewModel;
    }
}
