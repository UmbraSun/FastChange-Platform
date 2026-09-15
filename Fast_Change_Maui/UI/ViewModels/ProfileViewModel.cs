using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.DTOs.Users;
using Core.Interfaces;

namespace UI.ViewModels;

public partial class ProfileViewModel : ObservableObject
{
    private readonly IUserService _userService;
    private readonly IAuthService _authService;

    [ObservableProperty]
    private CurrentUserResponseDto? user;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    public string Email => User?.Email ?? string.Empty;

    public string Initial => string.IsNullOrWhiteSpace(User?.Email) ? "U" : User.Email[0].ToString().ToUpperInvariant();

    public string VerificationStatus => User?.IsVerified == true ? "Verified" : "Not verified";

    public ProfileViewModel(
        IUserService userService,
        IAuthService authService)
    {
        _userService = userService;
        _authService = authService;
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;

            User = await _userService.GetCurrentUserAsync();

            OnPropertyChanged(nameof(Email));
            OnPropertyChanged(nameof(Initial));
            OnPropertyChanged(nameof(VerificationStatus));
        }
        catch (Exception)
        {
            ErrorMessage = "Unable to load profile.";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task LogoutAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;

            await _authService.LogoutAsync();

            User = null;

            OnPropertyChanged(nameof(Email));
            OnPropertyChanged(nameof(Initial));
            OnPropertyChanged(nameof(VerificationStatus));

            await Shell.Current.GoToAsync("//login");
        }
        catch (Exception)
        {
            ErrorMessage = "Unable to sign out.";
        }
        finally
        {
            IsBusy = false;
        }
    }
}