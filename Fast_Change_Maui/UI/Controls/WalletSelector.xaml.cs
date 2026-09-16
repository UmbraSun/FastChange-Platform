using System.Collections;
using Core.DTOs.Wallets;

namespace UI.Controls;

public partial class WalletSelector : ContentView
{
    public static readonly BindableProperty WalletsProperty =
        BindableProperty.Create(
            nameof(Wallets),
            typeof(IEnumerable),
            typeof(WalletSelector),
            default(IEnumerable));

    public static readonly BindableProperty SelectedWalletProperty =
        BindableProperty.Create(
            nameof(SelectedWallet),
            typeof(WalletDto),
            typeof(WalletSelector),
            default(WalletDto),
            BindingMode.TwoWay);

    public static readonly BindableProperty SelectorTitleProperty =
        BindableProperty.Create(
            nameof(SelectorTitle),
            typeof(string),
            typeof(WalletSelector),
            "Wallet");

    public static readonly BindableProperty IsExpandedProperty =
        BindableProperty.Create(
            nameof(IsExpanded),
            typeof(bool),
            typeof(WalletSelector),
            false);

    public IEnumerable? Wallets
    {
        get => (IEnumerable?)GetValue(WalletsProperty);
        set => SetValue(WalletsProperty, value);
    }

    public WalletDto? SelectedWallet
    {
        get => (WalletDto?)GetValue(SelectedWalletProperty);
        set => SetValue(SelectedWalletProperty, value);
    }

    public string SelectorTitle
    {
        get => (string)GetValue(SelectorTitleProperty);
        set => SetValue(SelectorTitleProperty, value);
    }

    public bool IsExpanded
    {
        get => (bool)GetValue(IsExpandedProperty);
        set => SetValue(IsExpandedProperty, value);
    }

    public WalletSelector()
    {
        InitializeComponent();
    }

    private void SelectedWallet_Tapped(object? sender, TappedEventArgs e)
    {
        if (Wallets is null) return;
        IsExpanded = !IsExpanded;
    }

    private void WalletItem_Tapped(object? sender, TappedEventArgs e)
    {
        if (sender is not TapGestureRecognizer recognizer || recognizer.CommandParameter is not WalletDto wallet)
            return;

        SelectedWallet = wallet;
        IsExpanded = false;
    }
}