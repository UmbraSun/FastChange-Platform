using UI.ViewModels;

namespace UI.Views.Sections;

public partial class WithdrawView : ContentView
{
    private readonly WithdrawViewModel _viewModel;

    public WithdrawView(WithdrawViewModel viewModel)
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