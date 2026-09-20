using UI.ViewModels;

namespace UI.Views.Sections;

public partial class DepositView : ContentView
{
    private readonly DepositViewModel _viewModel;

    public DepositView(DepositViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    public async Task LoadAsync()
    {
        await _viewModel.LoadCommand.ExecuteAsync(null);
    }
}