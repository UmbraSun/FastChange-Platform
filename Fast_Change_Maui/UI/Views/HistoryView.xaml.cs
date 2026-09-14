using UI.ViewModels;

namespace UI.Views.Sections;

public partial class HistoryView : ContentView
{
    private readonly HistoryViewModel _viewModel;

    public HistoryView(HistoryViewModel viewModel)
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