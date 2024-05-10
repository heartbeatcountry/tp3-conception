using System.Reflection;
using System.Windows;

using CineQuebec.Application.Interfaces.Services.Identity;
using CineQuebec.Windows.Interfaces.ViewModels.Components;
using CineQuebec.Windows.ViewModels.Screens;

using Stylet;

namespace CineQuebec.Windows.ViewModels.Components;

public class HeaderViewModel(
    INavigationController navigationController,
    IUtilisateurAuthenticationService authenticationService,
    IGestionnaireExceptions gestionnaireExceptions) : Screen, IHeaderViewModel
{
    private readonly MethodInfo _navigateToMethodInfo =
        typeof(INavigationController).GetMethod(nameof(INavigationController.NavigateTo))!;

    public Type? PreviousView { private get; set; }
    public object? PreviousViewData { private get; set; }

    public bool CanGoBack => PreviousView?.GetInterface(nameof(IScreen)) != null;
    public bool CanGoToHome => CanGoBack;
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

    public void GoToHome()
    {
        if (!CanGoBack)
        {
            return;
        }

        navigationController.NavigateTo<HomeViewModel>();
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