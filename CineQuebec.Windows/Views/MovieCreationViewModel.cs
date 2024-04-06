using CineQuebec.Application.Interfaces.Services;

using Stylet;

namespace CineQuebec.Windows.Views;

public class MovieCreationViewModel(
    INavigationController navigationController,
    IFilmCreationService filmCreationService) : Screen
{
    public void NavigateToAdminHome()
    {
        navigationController.NavigateTo<AdminHomeViewModel>();
    }
}