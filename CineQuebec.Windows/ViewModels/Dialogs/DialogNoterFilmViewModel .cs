using CineQuebec.Windows.Interfaces.ViewModels.Dialogs;

using Stylet;

namespace CineQuebec.Windows.ViewModels.Dialogs;

public class DialogNoterFilmViewModel : Screen, IDialogNoterFilmViewModel
{
    private byte _noteFilm;

    public static List<byte> NotesPossibles => Enumerable
        .Range(Domain.Entities.Films.NoteFilm.NoteMinimum, Domain.Entities.Films.NoteFilm.NoteMaximum)
        .Select(Convert.ToByte).ToList();

    public bool AValide { get; private set; }

    public byte NoteFilm
    {
        get => _noteFilm;
        set => SetAndNotify(ref _noteFilm, value);
    }

    public void Annuler()
    {
        AValide = false;
        RequestClose(false);
    }

    public void Valider()
    {
        AValide = true;
        RequestClose(true);
    }
}