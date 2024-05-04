using Stylet;

namespace CineQuebec.Windows.ViewModels;

public class RootViewModel : Conductor<IScreen>, INavigationControllerDelegateFn
{
    public void NavigateTo(IScreen screen)
    {
        ActivateItem(screen);
    }
}