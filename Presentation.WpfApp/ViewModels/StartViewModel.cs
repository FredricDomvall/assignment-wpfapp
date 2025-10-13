using CommunityToolkit.Mvvm.ComponentModel;

namespace Presentation.WpfApp.ViewModels;

public partial class StartViewModel : ObservableObject
{
    [ObservableProperty]
    private string _title = "Start View Model";
}
