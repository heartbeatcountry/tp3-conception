using CineQuebec.Windows.Interfaces.ViewModels.Dialogs;
using CineQuebec.Windows.ViewModels.Dialogs;

namespace CineQuebec.Windows.Interfaces;

public interface IDialogFactory
{
    IDialogNomPrenomViewModel CreateDialogNomPrenom();
    IDialogNomAffichageViewModel CreateDialogNomAffichage();
    IDialogNouvelleSalleViewModel CreateDialogNouvelleSalle();
    IDialogInscriptionUtilisateurViewModel CreateDialogInscriptionUtilisateur();

    IDialogNoterFilmViewModel CreateDialogNoterFilm();
}