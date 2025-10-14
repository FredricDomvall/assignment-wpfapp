using Infrastructure.Configurations;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Presentation.WpfApp.ViewModels;
using Presentation.WpfApp.ViewModels.CategoryViewModels;
using Presentation.WpfApp.ViewModels.ManufacturerViewModels;
using Presentation.WpfApp.ViewModels.ProductViewModels;
using Presentation.WpfApp.Views;
using Presentation.WpfApp.Views.CategoryViews;
using Presentation.WpfApp.Views.ManufacturerViews;
using Presentation.WpfApp.Views.ProductViews;
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
                services.AddSingleton<FileSources>();

                services.AddSingleton<IJsonFileRepository<Product> , JsonFileRepository<Product>>();
                services.AddSingleton<IProductService, ProductService>();

                services.AddSingleton<IJsonFileRepository<Category>, JsonFileRepository<Category>>();
                services.AddSingleton<ICategoryService, CategoryService>();

                services.AddSingleton<IJsonFileRepository<Manufacturer>, JsonFileRepository<Manufacturer>>();
                services.AddSingleton<IManufacturerService, ManufacturerService>();

                services.AddSingleton<MainViewModel>();
                services.AddSingleton<MainWindow>();

                services.AddTransient<StartViewModel>();
                services.AddTransient<StartView>();

                services.AddTransient<ProductCreateViewModel>();
                services.AddTransient<ProductCreateView>();

                services.AddTransient<ProductListViewModel>();
                services.AddTransient<ProductListView>();

                services.AddTransient<ProductUpdateViewModel>();
                services.AddTransient<ProductUpdateView>();

                services.AddTransient<ProductDeleteViewModel>();

                services.AddTransient<CategoryCreateViewModel>();
                services.AddTransient<CategoryCreateView>();

                services.AddTransient<CategoryListViewModel>();
                services.AddTransient<CategoryListView>();

                services.AddTransient<CategoryUpdateViewModel>();
                services.AddTransient<CategoryUpdateView>();

                services.AddTransient<CategoryDeleteViewModel>();

                services.AddTransient<ManufacturerCreateViewModel>();
                services.AddTransient<ManufacturerCreateView>();

                services.AddTransient<ManufacturerListViewModel>();
                services.AddTransient<ManufacturerListView>();

                services.AddTransient<ManufacturerUpdateViewModel>();
                services.AddTransient<ManufacturerUpdateView>();

                services.AddTransient<ManufacturerDeleteViewModel>();
            })
            .Build();
    }
    protected override async void OnStartup(StartupEventArgs e)
    {
        await _host.StartAsync();

        base.OnStartup(e);

        var mainViewModel = _host.Services.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _host.Services.GetRequiredService<StartViewModel>();

        var mainWindow = _host.Services.GetRequiredService<MainWindow>();
        mainWindow.DataContext = mainViewModel;

        mainWindow.Show(); 
    }
}
