using CineQuebec.Windows.Views.Components;

using Stylet;

namespace CineQuebec.Windows.Views;

public class AdminHomeViewModel(INavigationController navigationController, HeaderViewModel headerViewModel) : Screen
{
    public HeaderViewModel HeaderViewModel => headerViewModel;

    private bool _navigationIsEnabled = true;

    public bool NavigationIsEnabled
    {
        get => _navigationIsEnabled;
        set => SetAndNotify(ref _navigationIsEnabled, value);
    }

    public void Logout()
    {
        navigationController.NavigateTo<LoginViewModel>();
    }

    public void NavigateToFilmManagement()
    {
        NavigationIsEnabled = false;
        navigationController.NavigateTo<AdminMovieListViewModel>();
    }
}