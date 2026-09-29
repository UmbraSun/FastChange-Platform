using UI.ViewModels;

namespace UI.Views.Sections;

public partial class AssistantView : ContentView
{
    private readonly AssistantViewModel _viewModel;

    public AssistantView(AssistantViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
        BindingContext = viewModel;
    }
}