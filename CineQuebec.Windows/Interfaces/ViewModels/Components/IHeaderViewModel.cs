using System.Windows;

namespace CineQuebec.Windows.Interfaces.ViewModels.Components;

public interface IHeaderViewModel
{
    Type? PreviousView { set; }
    object? PreviousViewData { set; }
    bool CanGoBack { get; }
    bool CanGoToHome { get; }
    Visibility BackButtonVisibility { get; }
    void GoBack();
    void Logout();
    void GoToHome();
}