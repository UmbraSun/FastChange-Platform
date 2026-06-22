using Core;
using UI.Views;

namespace Core;

public static class NavigationHelper
{
    public static void NavigateToDashboard()
    {
        // This would be used to navigate from registration to dashboard
        // In a real implementation, this would use the AppShell's navigation system
        var appShell = Application.Current?.MainPage as AppShell;
        if (appShell != null)
        {
            // Navigate to dashboard page
            // Note: Actual navigation implementation depends on your specific routing setup
        }
    }
}
