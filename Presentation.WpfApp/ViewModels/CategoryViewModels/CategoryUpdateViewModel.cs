using CommunityToolkit.Mvvm.ComponentModel;

namespace Presentation.WpfApp.ViewModels.CategoryViewModels;
public partial class CategoryUpdateViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    public CategoryUpdateViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    [ObservableProperty]
    private string _title = "Update Category";
}
