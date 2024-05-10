using CineQuebec.Windows.Interfaces.ViewModels.Components;

using Stylet;

namespace CineQuebec.Windows.Interfaces.ViewModels.Screens.User;

public interface IPreferencesViewModel : IScreen
{
    public IHeaderViewModel HeaderViewModel { get; }
    public IActeursFavorisViewModel ActeursFavorisViewModel { get; }
    public IRealisateursFavorisViewModel RealisateursFavorisViewModel { get; }
    public ICategoriesPrefereesViewModel CategoriesPrefereesViewModel { get; }
}