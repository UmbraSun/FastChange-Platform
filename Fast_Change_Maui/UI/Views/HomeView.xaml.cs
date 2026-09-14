using UI.ViewModels;

namespace UI.Views.Sections;

public partial class HomeView : ContentView
{
    private readonly HomeViewModel _viewModel;

    public HomeView(HomeViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    public Task LoadAsync()
    {
        return _viewModel.LoadCommand.ExecuteAsync(null);
    }
}