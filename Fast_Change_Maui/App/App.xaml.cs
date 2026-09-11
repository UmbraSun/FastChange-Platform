namespace App;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var services = Handler?.MauiContext?.Services
            ?? throw new InvalidOperationException("MAUI service provider is not available.");

        var shell = services.GetRequiredService<AppShell>();
        return new Window(shell);
    }
}