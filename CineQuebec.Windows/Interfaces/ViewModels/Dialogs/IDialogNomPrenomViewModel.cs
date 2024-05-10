using Stylet;

namespace CineQuebec.Windows.Interfaces.ViewModels.Dialogs;

public interface IDialogNomPrenomViewModel : IScreen
{
    bool AValide { get; }
    string Nom { get; set; }
    string Prenom { get; set; }
    void Annuler();
    void Valider();
}