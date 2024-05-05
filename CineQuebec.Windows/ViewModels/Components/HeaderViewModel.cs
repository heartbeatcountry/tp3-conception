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
    private readonly MethodInfo _navigateToMethodInfo =
        typeof(INavigationController).GetMethod(nameof(INavigationController.NavigateTo))!;

    public Type? PreviousView { private get; set; }
    public object? PreviousViewData { private get; set; }

    private bool CanGoBack => PreviousView?.GetInterface(nameof(IScreen)) != null;
    public Visibility BackButtonVisibility => CanGoBack ? Visibility.Visible : Visibility.Collapsed;

    public void GoBack()
    {
        if (!CanGoBack)
        {
            return;
        }

        _navigateToMethodInfo
            .MakeGenericMethod(PreviousView!)
            .Invoke(navigationController, [PreviousViewData]);
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