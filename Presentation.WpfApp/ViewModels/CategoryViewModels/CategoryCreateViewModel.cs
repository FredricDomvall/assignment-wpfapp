using CommunityToolkit.Mvvm.ComponentModel;

namespace Presentation.WpfApp.ViewModels.CategoryViewModels;
public partial class CategoryCreateViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    public CategoryCreateViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    [ObservableProperty]
    private string _title = "Create New Category";
}
