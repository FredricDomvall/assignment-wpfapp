using Microsoft.Extensions.Hosting;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Presentation.WpfApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IHost _host;
        public App()
        {
            _host = Host.CreateDefaultBuilder().ConfigureServices().Build();
        }
    }

}
