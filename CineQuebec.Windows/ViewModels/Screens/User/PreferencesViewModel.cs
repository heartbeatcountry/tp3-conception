using CineQuebec.Windows.Interfaces.ViewModels.Components;
using CineQuebec.Windows.Interfaces.ViewModels.Screens;
using CineQuebec.Windows.Interfaces.ViewModels.Screens.User;

using Stylet;

namespace CineQuebec.Windows.ViewModels.Screens.User;

public class PreferencesViewModel : Screen, IPreferencesViewModel
{
    public PreferencesViewModel(IHeaderViewModel headerViewModel, IActeursFavorisViewModel acteursFavorisViewModel,
        IRealisateursFavorisViewModel realisateursFavorisViewModel,
        ICategoriesPrefereesViewModel categoriesPrefereesViewModel)
    {
        HeaderViewModel = headerViewModel;
        ActeursFavorisViewModel = acteursFavorisViewModel;
        RealisateursFavorisViewModel = realisateursFavorisViewModel;
        CategoriesPrefereesViewModel = categoriesPrefereesViewModel;

        HeaderViewModel.PreviousView = typeof(IHomeViewModel);
    }

    public IHeaderViewModel HeaderViewModel { get; }

    public IActeursFavorisViewModel ActeursFavorisViewModel { get; }
    public IRealisateursFavorisViewModel RealisateursFavorisViewModel { get; }
    public ICategoriesPrefereesViewModel CategoriesPrefereesViewModel { get; }
}