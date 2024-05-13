using Stylet;

namespace CineQuebec.Windows.Interfaces.ViewModels.Dialogs;

public interface IDialogNoterFilmViewModel : IScreen
{
    bool AValide { get; }
    byte NoteFilm { get; set; }
    void Annuler();
    void Valider();
}