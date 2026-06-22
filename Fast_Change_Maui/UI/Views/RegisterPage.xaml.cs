using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using UI.ViewModels;

namespace UI.Views;

public partial class RegisterPage : ContentPage
{
    private readonly RegisterViewModel _viewModel;

    public RegisterPage(RegisterViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Clear the form when appearing
        _viewModel.Email = string.Empty;
        _viewModel.Password = string.Empty;
    }

    // Event handler for registration button click
    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        // The actual registration is handled by the command binding
        // This method exists to satisfy XAML but the real work happens in the ViewModel
    }
}
