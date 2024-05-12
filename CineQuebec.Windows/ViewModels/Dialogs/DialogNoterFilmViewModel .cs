using System.Windows;

using CineQuebec.Application.Interfaces.Services.Films;
using CineQuebec.Application.Interfaces.Services.Preferences;
using CineQuebec.Application.Interfaces.Services.Projections;
using CineQuebec.Application.Records.Projections;
using CineQuebec.Application.Services.Identity;
using CineQuebec.Domain.Entities.Films;
using CineQuebec.Domain.Entities.Utilisateurs;
using CineQuebec.Domain.Interfaces.Entities.Films;
using CineQuebec.Windows.Interfaces.ViewModels.Dialogs;

using Stylet;

namespace CineQuebec.Windows.ViewModels.Dialogs;

public class DialogNoterFilmViewModel : Screen, IDialogNoterFilmViewModel
{
    private List<byte> _notePossibles;
    private byte _noteFilm;
   

    public bool AValide { get; private set; }


    public List<byte> NotesPossibles => Enumerable
       .Range(Domain.Entities.Films.NoteFilm.NoteMinimum, Domain.Entities.Films.NoteFilm.NoteMaximum)
       .Select(Convert.ToByte).ToList();


    public byte NoteFilm
    {
        get => _noteFilm;
        set
        {
            if (_noteFilm != value)
            {
                _noteFilm = value;
                NotifyOfPropertyChange(() => NoteFilm);
            }
        }
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