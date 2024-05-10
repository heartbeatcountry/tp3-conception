using System.Windows.Controls;

using CineQuebec.Application.Records.Films;
using CineQuebec.Windows.Records;

using Stylet;

namespace CineQuebec.Windows.Interfaces.ViewModels.Components;

public interface IRealisateursFavorisViewModel
{
    public byte NbMaxRealisateursFavoris { get; }
    public byte NbRealisateursSelectionnes { get; }
    public BindableCollection<SelectedItemWrapper<RealisateurDto>> Realisateurs { get; }
    public bool CanSauvegarder { get; }

    public Task Sauvegarder();
    public void OnSelectionChanged(SelectionChangedEventArgs evt);
}