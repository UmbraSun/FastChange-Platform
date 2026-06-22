using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.Dto.Auth;
using Core.Services;

namespace UI.ViewModels;

public partial class RegisterViewModel : ObservableObject
{
    private readonly IAuthService _authService;
    private readonly IAlertService _alertService;

    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private bool _isBusy;

    public RegisterViewModel(IAuthService authService, IAlertService alertService)
    {
        _authService = authService;
        _alertService = alertService;
    }

[RelayCommand()]
    public async Task RegisterAsyncCommand()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;

            var request = new RegisterRequestDto(Email, Password);
            var result = await _authService.RegisterAsync(request);

            // Clean, mockable execution line compliant with .NET 10 specifications
            await _alertService.ShowAlertAsync(
                "Success",
                "Account created with default multi-currency wallets!");
                
            // Navigate to dashboard after successful registration
            // This would normally navigate using the application's navigation system
        }
        catch (ApplicationException ex)
        {
            await _alertService.ShowAlertAsync("Validation Error", ex.Message);
        }
        catch (Exception)
        {
            await _alertService.ShowAlertAsync("Error", "Could not connect to the server.");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
