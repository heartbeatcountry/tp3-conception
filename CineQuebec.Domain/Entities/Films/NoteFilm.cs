using System.Diagnostics.CodeAnalysis;

using CineQuebec.Domain.Entities.Abstract;
using CineQuebec.Domain.Interfaces.Entities.Films;

namespace CineQuebec.Domain.Entities.Films;

public class NoteFilm : Entite, INoteFilm
{
    public const byte NoteMinimum = 1;
    public const byte NoteMaximum = 10;

    public NoteFilm(Guid pIdUtilisateur, Guid pIdFilm, byte pNote)
    {
        SetUtilisateur(pIdUtilisateur);
        SetFilm(pIdFilm);
        SetNoteFilm(pNote);
    }

    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private NoteFilm(Guid id, Guid pIdUtilisateur, Guid pIdFilm, byte pNote) : this(pIdUtilisateur, pIdFilm, pNote)
    {
        // Constructeur avec identifiant pour Entity Framework Core
        SetId(id);
    }

    public Guid IdUtilisateur { get; private set; } = Guid.Empty;
    public Guid IdFilm { get; private set; } = Guid.Empty;
    public byte Note { get; private set; } = NoteMinimum;

    public void SetUtilisateur(Guid pIdUtilisateur)
    {
        if (pIdUtilisateur == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(pIdUtilisateur), "Le guid de l'utilisateur ne peut pas être nul.");
        }

        IdUtilisateur = pIdUtilisateur;
    }

    public void SetFilm(Guid pIdFilm)
    {
        if (pIdFilm == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(pIdFilm), "Le guid du film ne peut pas être nul.");
        }

        IdFilm = pIdFilm;
    }


    public void SetNoteFilm(byte pNoteObtenue)
    {
        if (pNoteObtenue is > NoteMaximum or < NoteMinimum)
        {
            throw new ArgumentOutOfRangeException(nameof(pNoteObtenue),
                $"La note doit être comprise entre {NoteMinimum} et {NoteMaximum}.");
        }

        Note = pNoteObtenue;
    }
}