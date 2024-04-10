using CineQuebec.Windows.Views.Components;

using Stylet;

namespace CineQuebec.Windows.Views;

public class AdminHomeViewModel(INavigationController navigationController, HeaderViewModel headerViewModel) : Screen
{
    public HeaderViewModel HeaderViewModel => headerViewModel;

    public void Logout()
    {
        navigationController.NavigateTo<LoginViewModel>();
    }

    public void NavigateToFilmManagement()
    {
        navigationController.NavigateTo<AdminMovieListViewModel>();
    }
}