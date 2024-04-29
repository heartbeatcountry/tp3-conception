using System.Windows;

using CineQuebec.Application.Interfaces.Services;
using CineQuebec.Domain.Entities.Utilisateurs;
using CineQuebec.Windows.Views.Components;

using Stylet;

namespace CineQuebec.Windows.Views;

public class AdminHomeViewModel(
    INavigationController navigationController,
    HeaderViewModel headerViewModel,
    IUtilisateurAuthenticationService utilisateurAuthenticationService) : Screen
{
    private bool _navigationIsEnabled = true;
    public HeaderViewModel HeaderViewModel => headerViewModel;

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
        navigationController.NavigateTo<AdminMovieListViewModel>();
    }
}