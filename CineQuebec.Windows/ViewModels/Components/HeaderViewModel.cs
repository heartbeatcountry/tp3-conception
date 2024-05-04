using System.Reflection;
using System.Windows;

using CineQuebec.Application.Interfaces.Services;
using CineQuebec.Windows.ViewModels.Screens;

using Stylet;

namespace CineQuebec.Windows.ViewModels.Components;

public class HeaderViewModel(
    INavigationController navigationController,
    IUtilisateurAuthenticationService authenticationService,
    IGestionnaireExceptions gestionnaireExceptions) : Screen
{
    public Type? PreviousView { get; set; }
    public object? PreviousViewData { get; set; }

    public bool CanGoBack => PreviousView?.GetInterface(nameof(IScreen)) != null;
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
        try
        {
            authenticationService.DeauthentifierThread();
        }
        catch (Exception ex)
        {
            gestionnaireExceptions.GererException(ex);
        }

        navigationController.NavigateTo<LoginViewModel>();
    }
}