using CineQuebec.Windows.Interfaces.ViewModels.Dialogs;

namespace CineQuebec.Windows.Interfaces;

public interface IDialogFactory
{
    IDialogNomPrenomViewModel CreateDialogNomPrenom();
    IDialogNomAffichageViewModel CreateDialogNomAffichage();
    IDialogNouvelleSalleViewModel CreateDialogNouvelleSalle();
    IDialogInscriptionUtilisateurViewModel CreateDialogInscriptionUtilisateur();
}