using Stylet;

namespace CineQuebec.Windows.Views;

public class RootViewModel : Conductor<IScreen>, INavigationControllerDelegateFn
{
    public void NavigateTo(IScreen screen)
    {
        ActivateItem(screen);
    }
}