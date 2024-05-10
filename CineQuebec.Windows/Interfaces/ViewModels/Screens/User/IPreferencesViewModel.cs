using CineQuebec.Windows.Interfaces.ViewModels.Components;

namespace CineQuebec.Windows.Interfaces.ViewModels.Screens.User;

public interface IPreferencesViewModel
{
    public IHeaderViewModel HeaderViewModel { get; }
    public IActeursFavorisViewModel ActeursFavorisViewModel { get; }
    public IRealisateursFavorisViewModel RealisateursFavorisViewModel { get; }
    public ICategoriesPrefereesViewModel CategoriesPrefereesViewModel { get; }
}