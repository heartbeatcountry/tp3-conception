using System.Windows.Controls;

using CineQuebec.Application.Records.Films;
using CineQuebec.Windows.Records;

using Stylet;

namespace CineQuebec.Windows.Interfaces.ViewModels.Components;

public interface ICategoriesPrefereesViewModel
{
    public byte NbMaxCategoriesPreferees { get; }
    public byte NbCategoriesSelectionnes { get; }
    public BindableCollection<SelectedItemWrapper<CategorieFilmDto>> Categories { get; }
    public bool CanSauvegarder { get; }

    public Task Sauvegarder();
    public void OnSelectionChanged(SelectionChangedEventArgs evt);
}