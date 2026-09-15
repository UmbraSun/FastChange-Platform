using UI.ViewModels;

namespace UI.Views.Sections;

public partial class ProfileView : ContentView
{
    private readonly ProfileViewModel _viewModel;

    public ProfileView(ProfileViewModel viewModel)
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