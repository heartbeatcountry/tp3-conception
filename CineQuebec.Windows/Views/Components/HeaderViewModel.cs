using System.Reflection;
using System.Windows;

using Stylet;

namespace CineQuebec.Windows.Views.Components;

public class HeaderViewModel(INavigationController navigationController) : Screen
{
    public Type? PreviousView { get; set; }
    public object? PreviousViewData { get; set; } = null;

    public bool CanGoBack => PreviousView != null && PreviousView.IsSubclassOf(typeof(Screen));
    public Visibility BackButtonVisibility => CanGoBack ? Visibility.Visible : Visibility.Collapsed;

    public void GoBack()
    {
        if (!CanGoBack)
        {
            return;
        }

        MethodInfo method = navigationController.GetType().GetMethod("NavigateTo")!;
        method.MakeGenericMethod(PreviousView!).Invoke(navigationController, [PreviousViewData]);
    }

    public void Logout()
    {
        navigationController.NavigateTo<LoginViewModel>();
    }
}