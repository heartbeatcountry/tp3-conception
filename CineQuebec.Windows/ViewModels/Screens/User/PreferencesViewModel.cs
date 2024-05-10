using CineQuebec.Windows.Interfaces.ViewModels.Components;
using CineQuebec.Windows.Interfaces.ViewModels.Screens.User;

using Stylet;

namespace CineQuebec.Windows.ViewModels.Screens.User;

public class PreferencesViewModel : Screen, IPreferencesViewModel
{
    public PreferencesViewModel(IHeaderViewModel headerViewModel, IActeursFavorisViewModel acteursFavorisViewModel)
    {
        HeaderViewModel = headerViewModel;
        ActeursFavorisViewModel = acteursFavorisViewModel;

        HeaderViewModel.PreviousView = typeof(HomeViewModel);
    }

    public IHeaderViewModel HeaderViewModel { get; }

    public IActeursFavorisViewModel ActeursFavorisViewModel { get; }
}