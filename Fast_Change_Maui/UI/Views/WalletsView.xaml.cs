using UI.ViewModels;

namespace UI.Views.Sections;

public partial class WalletsView : ContentView
{
    public WalletsView(WalletsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}