using Core.DTOs.Transfers;
using UI.ViewModels;

namespace UI.Views.Sections;

public partial class TransferView : ContentView
{
    private readonly TransferViewModel _viewModel;

    public TransferView(TransferViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    public async Task LoadAsync()
    {
        await _viewModel.LoadCommand.ExecuteAsync(null);
    }

    private void Recipient_Tapped(
        object? sender,
        TappedEventArgs e)
    {
        if (sender is not Border border || border.BindingContext is not TransferRecipientDto recipient)
            return;

        _viewModel.SelectRecipient(recipient);
    }
}