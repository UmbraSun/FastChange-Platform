namespace UI.Controls;

public partial class BottomNavigation : ContentView
{
    private static readonly Color ActiveColor = Color.FromArgb("#D4AF37");
    private static readonly Color InactiveColor = Color.FromArgb("#848E9C");

    public event EventHandler? HomeSelected;
    public event EventHandler? WalletsSelected;
    public event EventHandler? ExchangeSelected;
    public event EventHandler? HistorySelected;
    public event EventHandler? ProfileSelected;

    public static readonly BindableProperty SelectedTabProperty =
        BindableProperty.Create(
            nameof(SelectedTab),
            typeof(string),
            typeof(BottomNavigation),
            "Home",
            propertyChanged: OnSelectedTabChanged);

    public string SelectedTab
    {
        get => (string)GetValue(SelectedTabProperty);
        set => SetValue(SelectedTabProperty, value);
    }

    public Color HomeColor => GetColor("Home");
    public Color WalletsColor => GetColor("Wallets");
    public Color ExchangeColor => GetColor("Exchange");
    public Color HistoryColor => GetColor("History");
    public Color ProfileColor => GetColor("Profile");

    public BottomNavigation()
    {
        InitializeComponent();
    }

    private static void OnSelectedTabChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var navigation = (BottomNavigation)bindable;

        navigation.OnPropertyChanged(nameof(HomeColor));
        navigation.OnPropertyChanged(nameof(WalletsColor));
        navigation.OnPropertyChanged(nameof(ExchangeColor));
        navigation.OnPropertyChanged(nameof(HistoryColor));
        navigation.OnPropertyChanged(nameof(ProfileColor));
    }

    private Color GetColor(string tab)
    {
        return string.Equals(
            SelectedTab,
            tab,
            StringComparison.Ordinal)
            ? ActiveColor
            : InactiveColor;
    }

    private void Home_Tapped(object? sender, TappedEventArgs e)
    {
        SelectedTab = "Home";
        HomeSelected?.Invoke(this, EventArgs.Empty);
    }

    private void Wallets_Tapped(object? sender, TappedEventArgs e)
    {
        SelectedTab = "Wallets";
        WalletsSelected?.Invoke(this, EventArgs.Empty);
    }

    private void Exchange_Tapped(object? sender, TappedEventArgs e)
    {
        SelectedTab = "Exchange";
        ExchangeSelected?.Invoke(this, EventArgs.Empty);
    }

    private void History_Tapped(object? sender, TappedEventArgs e)
    {
        SelectedTab = "History";
        HistorySelected?.Invoke(this, EventArgs.Empty);
    }

    private void Profile_Tapped(object? sender, TappedEventArgs e)
    {
        SelectedTab = "Profile";
        ProfileSelected?.Invoke(this, EventArgs.Empty);
    }
}