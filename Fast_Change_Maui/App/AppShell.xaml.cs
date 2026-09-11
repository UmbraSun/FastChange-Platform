using App.Services;

namespace App;

public partial class AppShell : Shell
{
    private readonly StartupNavigationService _startupNavigationService;
    private bool _startupCompleted;

    public AppShell(StartupNavigationService startupNavigationService)
    {
        InitializeComponent();

        _startupNavigationService = startupNavigationService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_startupCompleted) return;

        _startupCompleted = true;
        await _startupNavigationService.NavigateAsync();
    }
}