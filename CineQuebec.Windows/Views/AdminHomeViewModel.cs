using Stylet;

namespace CineQuebec.Windows.Views;

public class AdminHomeViewModel(INavigationController navigationController) : Screen
{
    public void Logout()
    {
        navigationController.NavigateTo<LoginViewModel>();
    }

    public void NavigateToMovieCreation()
    {
        navigationController.NavigateTo<MovieCreationViewModel>();
    }
}