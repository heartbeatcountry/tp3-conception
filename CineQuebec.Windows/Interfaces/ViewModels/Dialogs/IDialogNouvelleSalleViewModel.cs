using Stylet;

namespace CineQuebec.Windows.Interfaces.ViewModels.Dialogs;

public interface IDialogNouvelleSalleViewModel : IScreen
{
    bool AValide { get; }
    string NumeroStr { get; set; }
    string CapaciteStr { get; set; }
    byte Numero { get; }
    ushort Capacite { get; }
    void Annuler();
    void Valider();
}