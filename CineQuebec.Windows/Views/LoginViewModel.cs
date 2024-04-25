using CineQuebec.Application.Interfaces.Services;

using Stylet;

namespace CineQuebec.Windows.Views;

public class LoginViewModel(INavigationController navigationController, IAuthenticationService authenticationService)
    : Screen
{
    public async Task NavigateToAdminHome()
    {
        bool succes = await authenticationService.AuthentifierThreadAsync("user", "pwd");
        navigationController.NavigateTo<AdminHomeViewModel>();
    }
}