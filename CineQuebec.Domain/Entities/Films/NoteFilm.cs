using System.Diagnostics.CodeAnalysis;

using CineQuebec.Domain.Entities.Abstract;
using CineQuebec.Domain.Interfaces.Entities.Films;

namespace CineQuebec.Domain.Entities.Films;

public class NoteFilm : Entite, INoteFilm
{
    public const byte NoteMinimum = 1;
    public const byte NoteMaximum = 10;

    public NoteFilm(Guid idUtilisateur, Guid idFilm, byte note)
    {
        SetIdUtilisateur(idUtilisateur);
        SetIdFilm(idFilm);
        SetNote(note);
    }

    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private NoteFilm(Guid id, Guid idUtilisateur, Guid idFilm, byte note) : this(idUtilisateur, idFilm, note)
    {
        // Constructeur avec identifiant pour Entity Framework Core
        SetId(id);
    }

    public Guid IdUtilisateur { get; private set; } = Guid.Empty;
    public Guid IdFilm { get; private set; } = Guid.Empty;
    public byte Note { get; private set; } = NoteMinimum;

    public void SetIdUtilisateur(Guid pIdUtilisateur)
    {
        if (pIdUtilisateur == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(pIdUtilisateur), "Le guid de l'utilisateur ne peut pas être nul.");
        }

        IdUtilisateur = pIdUtilisateur;
    }

    public void SetIdFilm(Guid pIdFilm)
    {
        if (pIdFilm == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(pIdFilm), "Le guid du film ne peut pas être nul.");
        }

        IdFilm = pIdFilm;
    }


    public void SetNote(byte pNoteObtenue)
    {
        if (pNoteObtenue is > NoteMaximum or < NoteMinimum)
        {
            throw new ArgumentOutOfRangeException(nameof(pNoteObtenue),
                $"La note doit être comprise entre {NoteMinimum} et {NoteMaximum}.");
        }

        Note = pNoteObtenue;
    }
}