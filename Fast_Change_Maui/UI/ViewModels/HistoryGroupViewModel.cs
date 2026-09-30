using System.Collections.ObjectModel;

namespace UI.ViewModels;

public sealed class HistoryGroupViewModel
{
    public DateTime Date { get; }

    public string DisplayDate => Date.ToString("MMM d, yyyy");

    public int Count => Items.Count;

    public string DisplayCount => $"{Count} {(Count == 1 ? "operation" : "operations")}";

    public ObservableCollection<HistoryItemViewModel> Items { get; }

    public HistoryGroupViewModel(DateTime date, IEnumerable<HistoryItemViewModel> items)
    {
        Date = date;
        Items = new ObservableCollection<HistoryItemViewModel>(items);
    }
}