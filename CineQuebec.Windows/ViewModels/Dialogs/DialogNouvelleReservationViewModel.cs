using CineQuebec.Application.Interfaces.Services.Films;
using CineQuebec.Application.Interfaces.Services.Projections;
using CineQuebec.Application.Records.Projections;
using CineQuebec.Windows.Interfaces.ViewModels.Dialogs;

using Stylet;

namespace CineQuebec.Windows.ViewModels.Dialogs;

public class DialogNouvelleReservationViewModel : Screen, IDialogNouvelleReservationViewModel
{
    private byte _nbBillets;

    public bool AValide { get; private set; }




    public byte NbBillets
    {
        get => _nbBillets;
        set => SetAndNotify(ref _nbBillets, value);
    }

  

    public void Annuler()
    {
        AValide = false;
        RequestClose(false);
    }

    public void Valider()
    {
        AValide = true;
        RequestClose(true);
    }
}