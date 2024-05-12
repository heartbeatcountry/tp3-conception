using System.Windows;

using CineQuebec.Application.Interfaces.Services.Identity;
using CineQuebec.Domain.Entities.Utilisateurs;
using CineQuebec.Windows.Interfaces.ViewModels.Components;
using CineQuebec.Windows.Interfaces.ViewModels.Screens;
using CineQuebec.Windows.Interfaces.ViewModels.Screens.Admin;
using CineQuebec.Windows.Interfaces.ViewModels.Screens.User;

using Stylet;

namespace CineQuebec.Windows.ViewModels.Screens;

public class HomeViewModel(
    INavigationController navigationController,
    IHeaderViewModel headerViewModel,
    IUtilisateurAuthenticationService utilisateurAuthenticationService) : Screen, IHomeViewModel
{
    private bool _navigationIsEnabled = true;
    public IHeaderViewModel HeaderViewModel => headerViewModel;

    public bool EstAdmin =>
        utilisateurAuthenticationService.ObtenirAutorisation()?.IsInRole(Role.Administrateur.ToString()) ?? false;

    public Visibility VisibilityAdmin => EstAdmin ? Visibility.Visible : Visibility.Collapsed;

    public bool NavigationIsEnabled
    {
        get => _navigationIsEnabled;
        set => SetAndNotify(ref _navigationIsEnabled, value);
    }

    public void NavigateToFilmManagement()
    {
        if (!EstAdmin)
        {
            return;
        }

        NavigationIsEnabled = false;
        navigationController.NavigateTo<IAdminMovieListViewModel>();
    }

    public void NavigateToPreferences()
    {
        NavigationIsEnabled = false;
        navigationController.NavigateTo<IPreferencesViewModel>();
    }

    public void NavigateToRecompenses()
    {
        NavigationIsEnabled = false;
        navigationController.NavigateTo<IAdminOffrirBilletsViewModel>();
    }
}