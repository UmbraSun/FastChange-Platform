using UI.ViewModels;

namespace UI.Views;

public partial class ExchangePage : ContentPage
{
    private readonly ExchangeViewModel _viewModel;

    private CancellationTokenSource? _previewDebounceCts;

    public ExchangePage(ExchangeViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadCommand.ExecuteAsync(null);
    }

    protected override void OnDisappearing()
    {
        CancelPreviewDebounce();
        base.OnDisappearing();
    }

    private async void AmountEntry_TextChanged(object? sender, TextChangedEventArgs e)
    {
        await DebouncePreviewAsync();
    }

    private async void WalletPicker_SelectedIndexChanged(object? sender, EventArgs e)
    {
        await DebouncePreviewAsync();
    }

    private async void AmountEntry_Completed(object? sender, EventArgs e)
    {
        CancelPreviewDebounce();
        await _viewModel.PreviewCommand.ExecuteAsync(null);
    }

    private async Task DebouncePreviewAsync()
    {
        CancelPreviewDebounce();

        var cts = new CancellationTokenSource();
        _previewDebounceCts = cts;

        try
        {
            await Task.Delay(500, cts.Token);
            await _viewModel.PreviewCommand.ExecuteAsync(null);
        }
        catch (OperationCanceledException)
        {
            // Expected when input changes before the debounce interval ends.
        }
    }

    private void CancelPreviewDebounce()
    {
        _previewDebounceCts?.Cancel();
        _previewDebounceCts?.Dispose();
        _previewDebounceCts = null;
    }
}