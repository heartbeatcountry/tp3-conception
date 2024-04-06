using CineQuebec.Windows.Views;
using Stylet;

namespace CineQuebec.Windows;

public interface INavigationController
{
    void NavigateToLogin();
}

public interface INavigationControllerDelegateFn
{
    void NavigateTo(IScreen screen);
}

public class NavigationController(Func<LoginViewModel> loginViewModelFactory)
    : INavigationController
{
    public INavigationControllerDelegateFn? Delegate { get; set; }

    public void NavigateToLogin()
    {
        Delegate?.NavigateTo(loginViewModelFactory());
    }
}