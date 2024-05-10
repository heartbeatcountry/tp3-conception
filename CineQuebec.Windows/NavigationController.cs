using Stylet;

using StyletIoC;

namespace CineQuebec.Windows;

public interface INavigationController
{
    void NavigateTo<TScreen>(object? data = null) where TScreen : IScreen;
}

public interface INavigationControllerDelegateFn
{
    void NavigateTo(IScreen screen);
}

public interface IScreenWithData : IScreen
{
    public void SetData(object data);
}

public class NavigationController(IContainer container)
    : INavigationController
{
    public INavigationControllerDelegateFn? Delegate { get; set; }

    public void NavigateTo<TScreen>(object? data = null) where TScreen : IScreen
    {
        TScreen? screen = container.Get<TScreen>();

        if (data != null && screen is IScreenWithData screenWithData)
        {
            screenWithData.SetData(data);
        }

        Delegate?.NavigateTo(screen);
    }
}