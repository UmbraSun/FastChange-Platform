namespace Core.Interfaces;

/// <summary>
/// Defines the interface for an alert service that provides methods to display alerts to the user.
/// </summary>
public interface IAlertService
{
    /// <summary>
    /// Displays an alert with the specified title, message, and cancel button text.
    /// </summary>
    /// <param name="title"></param>
    /// <param name="message"></param>
    /// <param name="cancel"></param>
    /// <returns></returns>
    Task ShowAlertAsync(string title, string message, string cancel = "OK");
}
