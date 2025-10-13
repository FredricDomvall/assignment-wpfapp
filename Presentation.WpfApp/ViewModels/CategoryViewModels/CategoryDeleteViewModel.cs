using CommunityToolkit.Mvvm.ComponentModel;

namespace Presentation.WpfApp.ViewModels.CategoryViewModels;
public partial class CategoryDeleteViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    public CategoryDeleteViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    [ObservableProperty]
    private string _title = "Delete Category From List";
}
