using CineQuebec.Windows.Views;

namespace CineQuebec.Windows;

public interface IDialogFactory
{
    DialogNomPrenomViewModel CreateDialogNomPrenom();
    DialogNomAffichageViewModel CreateDialogNomAffichage();
}