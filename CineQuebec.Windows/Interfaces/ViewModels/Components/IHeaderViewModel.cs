using System.Windows;

using Stylet;

namespace CineQuebec.Windows.Interfaces.ViewModels.Components;

public interface IHeaderViewModel : IScreen
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