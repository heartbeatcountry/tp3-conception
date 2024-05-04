using CineQuebec.Windows.ViewModels.Dialogs;

namespace CineQuebec.Windows;

public interface IDialogFactory
{
    DialogNomPrenomViewModel CreateDialogNomPrenom();
    DialogNomAffichageViewModel CreateDialogNomAffichage();
    DialogNouvelleSalleViewModel CreateDialogNouvelleSalle();
    DialogInscriptionUtilisateurViewModel CreateDialogInscriptionUtilisateur();
}