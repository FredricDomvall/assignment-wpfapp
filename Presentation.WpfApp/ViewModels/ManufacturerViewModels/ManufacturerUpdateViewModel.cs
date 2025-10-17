using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace Presentation.WpfApp.ViewModels.ManufacturerViewModels;

public partial class ManufacturerUpdateViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IManufacturerService _manufacturerService;

    public ManufacturerUpdateViewModel(IServiceProvider serviceProvider, IManufacturerService manufacturerService)
    {
        _serviceProvider = serviceProvider;
        _manufacturerService = manufacturerService;
    }

    [ObservableProperty]
    private string _title = "Update Manufacturer";

    [ObservableProperty]
    public Manufacturer? _currentManufacturerDetails;

    [RelayCommand]
    private async Task UpdateManufacturer()
    {
        var updateResult = await _manufacturerService.UpdateManufacturerInListByIdAsync(CurrentManufacturerDetails!);
        if (updateResult.Statement is false)
            CurrentManufacturerDetails = updateResult.Outcome;

        MessageBox.Show(updateResult.Answer);

        await _manufacturerService.LoadListFromFileAsync();
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        var manufacturerListViewModel = _serviceProvider.GetRequiredService<ManufacturerListViewModel>();
        mainViewModel.CurrentViewModel = manufacturerListViewModel;
    }
    [RelayCommand]
    private void NavigateToManufacturerListView()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        var manufacturerListViewModel = _serviceProvider.GetRequiredService<ManufacturerListViewModel>();
        mainViewModel.CurrentViewModel = manufacturerListViewModel;
    }
}
