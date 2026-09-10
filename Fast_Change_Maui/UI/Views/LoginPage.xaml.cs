using UI.ViewModels;

namespace UI.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}