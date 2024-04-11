using CineQuebec.Windows.Views.Components;

using Stylet;

namespace CineQuebec.Windows.Views;

public class AdminHomeViewModel(INavigationController navigationController, HeaderViewModel headerViewModel) : Screen
{
    private bool _navigationIsEnabled = true;
    public HeaderViewModel HeaderViewModel => headerViewModel;

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