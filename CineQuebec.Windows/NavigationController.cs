using Stylet;
using StyletIoC;

namespace CineQuebec.Windows;

public interface INavigationController
{
    void NavigateTo<TScreen>() where TScreen : IScreen;
}

public interface INavigationControllerDelegateFn
{
    void NavigateTo(IScreen screen);
}

public class NavigationController(IContainer container)
    : INavigationController
{
    public INavigationControllerDelegateFn? Delegate { get; set; }

    public void NavigateTo<TScreen>() where TScreen : IScreen
    {
        var screen = container.Get<TScreen>();
        Delegate?.NavigateTo(screen);
    }
}