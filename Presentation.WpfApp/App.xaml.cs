using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Presentation.WpfApp.ViewModels;
using Presentation.WpfApp.ViewModels.CategoryViewModels;
using Presentation.WpfApp.ViewModels.ManufacturerViewModels;
using Presentation.WpfApp.ViewModels.ProductViewModels;
using System.Windows;

namespace Presentation.WpfApp;

public partial class App : Application
{
    private IHost _host;
    public App()
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureServices(services => 
            {
                services.AddSingleton<MainViewModel>();
                services.AddSingleton<MainWindow>();

                services.AddTransient<StartViewModel>();

                services.AddTransient<ProductCreateViewModel>();
                services.AddTransient<ProductListViewModel>();
                services.AddTransient<ProductUpdateViewModel>();
                services.AddTransient<ProductDeleteViewModel>();

                services.AddTransient<CategoryCreateViewModel>();
                services.AddTransient<CategoryListViewModel>();
                services.AddTransient<CategoryUpdateViewModel>();
                services.AddTransient<CategoryDeleteViewModel>();

                services.AddTransient<ManufacturerCreateViewModel>();
                services.AddTransient<ManufacturerListViewModel>();
                services.AddTransient<ManufacturerUpdateViewModel>();
                services.AddTransient<ManufacturerDeleteViewModel>();

            })
            .Build();
    }
    protected override async void OnStartup(StartupEventArgs e)
    {
        await _host.StartAsync();

        base.OnStartup(e);

        var mainViewModel = _host.Services.GetRequiredService<MainViewModel>();
        mainviewModel.CurrentViewModel = _host.Services.GetRequiredService<StartViewModel>();

        var mainWindow = _host.Services.GetRequiredService<MainWindow>();
        mainWindow.DataContext = mainViewModel;

        mainWindow.Show(); 
    }
}
