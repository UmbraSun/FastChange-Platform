using Core.Services;

namespace UI.Services;

public sealed class AlertService : IAlertService
{
    public async Task ShowAlertAsync(string title, string message, string cancel = "OK")
    {
        if (Shell.Current?.CurrentPage is Page activePage)
        {
            // Utilizing the new non-deprecated .NET 10 asynchronous alert engine
            await activePage.DisplayAlertAsync(title, message, cancel);
        }
    }
}
