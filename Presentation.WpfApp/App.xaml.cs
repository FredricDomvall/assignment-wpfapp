using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Presentation.WpfApp.ViewModels;
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
            })
            .Build();
    }
}
