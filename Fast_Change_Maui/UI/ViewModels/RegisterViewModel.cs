using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.DTOs.Auth;
using Core.Interfaces;
using Refit;
using System.Net;

namespace UI.ViewModels;

public partial class RegisterViewModel : ObservableObject
{
    private readonly IAuthService _authService;

    public RegisterViewModel(IAuthService authService)
    {
        _authService = authService;
    }

    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;

    [ObservableProperty]
    private string confirmPassword = string.Empty;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    [ObservableProperty]
    private bool isBusy;

    [RelayCommand]
    private async Task RegisterAsync()
    {
        if (IsBusy)
        {
            return;
        }

        ErrorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(Email) ||
            string.IsNullOrWhiteSpace(Password) ||
            string.IsNullOrWhiteSpace(ConfirmPassword))
        {
            ErrorMessage = "All fields are required.";
            return;
        }

        if (!string.Equals(Password, ConfirmPassword, StringComparison.Ordinal))
        {
            ErrorMessage = "Passwords do not match.";
            return;
        }

        try
        {
            IsBusy = true;
            await _authService.RegisterAsync(new RegisterRequestDto(Email.Trim(), Password));
            await Shell.Current.GoToAsync("//login");
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
    private async Task OpenLoginAsync()
    {
        await Shell.Current.GoToAsync("//login");
    }

    private static string GetErrorMessage(Exception exception)
    {
        return exception switch
        {
            ApiException
            {
                StatusCode: HttpStatusCode.Conflict
            } => "An account with this email already exists.",
            
            ApiException
            {
                StatusCode: HttpStatusCode.UnprocessableEntity
            } => "Please check the entered data.",
            
            HttpRequestException => "Unable to connect to the server.",
            
            _ => "Something went wrong. Please try again."
        };
    }
}