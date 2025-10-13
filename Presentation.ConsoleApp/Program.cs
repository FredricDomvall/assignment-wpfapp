using Infrastructure.Configurations;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Presentation.ConsoleApp.Menus;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<FileSources>();
        services.AddSingleton<IJsonFileRepository<Product>, JsonFileRepository<Product>>();
        services.AddSingleton<IJsonFileRepository<Category>, JsonFileRepository<Category>>();

        services.AddSingleton<IProductService, ProductService>();
        services.AddSingleton<ICategoryService, CategoryService>();

        services.AddTransient<ProductMenu>();
        services.AddTransient<MainMenu>();
        
    })
    .Build();

MainMenu mainMenu = host.Services.GetRequiredService<MainMenu>();
await mainMenu.Run();
