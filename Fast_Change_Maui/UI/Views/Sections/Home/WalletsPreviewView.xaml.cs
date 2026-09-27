namespace UI.Views.Sections.Home;

public partial class WalletsPreviewView : ContentView
{
    public event EventHandler? ViewAllRequested;

    public WalletsPreviewView()
    {
        InitializeComponent();
    }

    private void ViewAll_Tapped(object? sender, TappedEventArgs e)
    {
        ViewAllRequested?.Invoke(this, EventArgs.Empty);
    }
}