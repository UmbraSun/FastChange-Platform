using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.Interfaces;

namespace UI.ViewModels;

public partial class AssistantViewModel : ObservableObject
{
    private readonly IChatService _chatService;

    [ObservableProperty]
    private string question = string.Empty;

    [ObservableProperty]
    private string answer = string.Empty;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    public bool HasAnswer => !string.IsNullOrWhiteSpace(Answer);

    public bool IsErrorVisible => !string.IsNullOrWhiteSpace(ErrorMessage);

    public bool IsAnswerVisible => !IsBusy && !IsErrorVisible && HasAnswer;

    public bool CanAsk => !IsBusy && !string.IsNullOrWhiteSpace(Question);

    public AssistantViewModel(IChatService chatService)
    {
        _chatService = chatService;
    }

    partial void OnQuestionChanged(string value)
    {
        OnPropertyChanged(nameof(CanAsk));
    }

    [RelayCommand]
    private async Task AskAsync()
    {
        if (IsBusy || string.IsNullOrWhiteSpace(Question))
            return;

        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;
            Answer = string.Empty;
            NotifyStateChanged();

            var question = Question.Trim();
            Answer = await _chatService.AskAsync(question);
            OnPropertyChanged(nameof(HasAnswer));
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception)
        {
            ErrorMessage = "Unable to get a response from the assistant.";
        }
        finally
        {
            IsBusy = false;

            NotifyStateChanged();
        }
    }

    private void NotifyStateChanged()
    {
        OnPropertyChanged(nameof(CanAsk));
        OnPropertyChanged(nameof(HasAnswer));
        OnPropertyChanged(nameof(IsErrorVisible));
        OnPropertyChanged(nameof(IsAnswerVisible));
    }
}