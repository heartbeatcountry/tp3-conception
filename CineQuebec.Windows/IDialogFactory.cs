using CineQuebec.Windows.Interfaces.ViewModels.Dialogs;
using CineQuebec.Windows.ViewModels.Dialogs;

namespace CineQuebec.Windows;

public interface IDialogFactory
{
    DialogNomPrenomViewModel CreateDialogNomPrenom();
    DialogNomAffichageViewModel CreateDialogNomAffichage();
    DialogNouvelleSalleViewModel CreateDialogNouvelleSalle();
    IDialogInscriptionUtilisateurViewModel CreateDialogInscriptionUtilisateur();
}