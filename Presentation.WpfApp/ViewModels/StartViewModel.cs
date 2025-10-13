using CommunityToolkit.Mvvm.ComponentModel;

namespace Presentation.WpfApp.ViewModels;

public partial class StartViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    public StartViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    [ObservableProperty]
    private string _title = "Start View Model";
}
