using CommunityToolkit.Mvvm.ComponentModel;

namespace Presentation.WpfApp.ViewModels.CategoryViewModels;
public partial class CategoryListViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    public CategoryListViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    [ObservableProperty]
    private string _title = "Category List";
}
