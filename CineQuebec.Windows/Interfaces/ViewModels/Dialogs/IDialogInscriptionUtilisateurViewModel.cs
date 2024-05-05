using System.Windows;

namespace CineQuebec.Windows.Interfaces.ViewModels.Dialogs;

public interface IDialogInscriptionUtilisateurViewModel
{
    bool InscriptionReussie { get; }
    string Nom { get; set; }
    string Prenom { get; set; }
    string Courriel { get; set; }
    string MotDePasse { set; }
    string ConfirmationMotDePasse { set; }
    void Annuler();
    Task Valider();
    void OnMdpChange(object sender, RoutedEventArgs e);
    void OnConfirmationMdpChange(object sender, RoutedEventArgs e);
    void Refresh();
}