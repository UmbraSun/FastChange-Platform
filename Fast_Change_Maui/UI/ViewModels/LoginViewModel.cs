using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.DTOs.Auth;
using Core.Interfaces;
using Refit;
using System.Net;

namespace UI.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly IAuthService _authService;

    public LoginViewModel(IAuthService authService)
    {
        _authService = authService;
    }

    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    [ObservableProperty]
    private bool isBusy;

    [RelayCommand]
    private async Task LoginAsync()
    {
        if (IsBusy) return;

        ErrorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Email and password are required.";
            return;
        }

        try
        {
            IsBusy = true;
            await _authService.LoginAsync(new LoginRequestDto(Email.Trim(), Password));
            await Shell.Current.GoToAsync("//dashboard");
        }
        catch (Exception ex)
        {
            ErrorMessage = GetErrorMessage(ex);
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task OpenRegisterAsync()
    {
        await Shell.Current.GoToAsync("//register");
    }

    private static string GetErrorMessage(Exception exception)
    {
        return exception switch
        {
            ApiException
            {
                StatusCode: HttpStatusCode.Unauthorized
            } => "Invalid email or password.",
            ApiException
            {
                StatusCode: HttpStatusCode.UnprocessableEntity
            } => "Please check the entered data.",
            HttpRequestException => "Unable to connect to the server.",

            _ => "Something went wrong. Please try again."
        };
    }
}