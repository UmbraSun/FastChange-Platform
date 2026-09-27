namespace UI.Views.Sections.Home;

public partial class HomeHeaderView : ContentView
{
    public event EventHandler? ProfileRequested;

    public HomeHeaderView()
    {
        InitializeComponent();
    }

    private void Profile_Tapped(object? sender, TappedEventArgs e)
    {
        ProfileRequested?.Invoke(this, EventArgs.Empty);
    }
}