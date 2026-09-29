namespace UI.Views.Sections.Home;

public partial class AssistantPreviewView : ContentView
{
    public event EventHandler? AssistantRequested;

    public AssistantPreviewView()
    {
        InitializeComponent();
    }

    private void AskAssistant_Clicked(object? sender, EventArgs e)
    {
        AssistantRequested?.Invoke(this, EventArgs.Empty);
    }
}