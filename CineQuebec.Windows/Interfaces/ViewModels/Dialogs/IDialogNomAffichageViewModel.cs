using Stylet;

namespace CineQuebec.Windows.Interfaces.ViewModels.Dialogs;

public interface IDialogNomAffichageViewModel : IScreen
{
    bool AValide { get; }
    string Nom { get; set; }
    void Annuler();
    void Valider();
}