using System.Windows.Controls;

using CineQuebec.Application.Records.Films;
using CineQuebec.Windows.Records;

using Stylet;

namespace CineQuebec.Windows.Interfaces.ViewModels.Components;

public interface IActeursFavorisViewModel
{
    public byte NbMaxActeursFavoris { get; }
    public byte NbActeursSelectionnes { get; }
    public BindableCollection<SelectedItemWrapper<ActeurDto>> Acteurs { get; }
    public bool CanSauvegarder { get; }

    public Task Sauvegarder();
    public void OnSelectionChanged(SelectionChangedEventArgs evt);
}