using Stylet;

namespace CineQuebec.Windows.Views;

public class LoginViewModel(INavigationController navigationController) : Screen
{
    public void NavigateToAdminHome()
    {
        navigationController.NavigateTo<AdminHomeViewModel>();
    }
}