using UI.ViewModels;

namespace UI.Views.Sections;

public partial class WalletsView : ContentView
{
    private readonly WalletsViewModel _viewModel;

    public WalletsView(WalletsViewModel viewModel)
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