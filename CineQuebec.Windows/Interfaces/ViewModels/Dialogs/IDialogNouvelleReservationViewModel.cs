using CineQuebec.Application.Records.Projections;

using Stylet;

namespace CineQuebec.Windows.Interfaces.ViewModels.Dialogs;

public interface IDialogNouvelleReservationViewModel : IScreenWithData
{
    bool AchatReussi { get; }
    ushort NbBillets { get; set; }
    BindableCollection<ushort> NbBilletsPossibles { get; }
    ProjectionDto Projection { get; }
    void Annuler();
    Task Valider();
}