using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CineQuebec.Domain.Entities.Abstract;
using CineQuebec.Domain.Interfaces.Entities;
using CineQuebec.Domain.Interfaces.Entities.Films;

namespace CineQuebec.Domain.Entities.Films
{
    public class NoteFilm : Entite, IComparable<NoteFilm>,  INoteFilm
    {
        public const byte NoteMoyenneMinimum = 0;
        public const byte NoteMoyenneMaximum = 10;

        public NoteFilm(Guid pIdUtilisateur, Guid pIdFilm, ushort pNote) {

            SetUtilisateur(pIdUtilisateur);
            SetFilm(pIdFilm);
            SetNoteFilm(pNote);

        }

        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        private NoteFilm(Guid id, Guid pIdUtilisateur, Guid pIdFilm, ushort pNote) : this(pIdUtilisateur,
        pIdFilm,
        pNote)
        {
            // Constructeur avec identifiant pour Entity Framework Core
            SetId(id);
        }

        public Guid IdUtilisateur { get; set; } = Guid.Empty; 
        public Guid IdFilm { get; set; } = Guid.Empty; 
        public ushort Note {  get; set; }


        public void SetUtilisateur(Guid pIdUtilisateur)
        {
            if (pIdUtilisateur == Guid.Empty)
            {
                throw new ArgumentNullException("Le guid de l'utilisateur ne peut pas être nul.", nameof(pIdUtilisateur));
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


        public void SetNoteFilm(ushort pNoteObtenue)
        {
            if (pNoteObtenue > NoteMoyenneMaximum || pNoteObtenue < NoteMoyenneMinimum)
            {
                throw new ArgumentOutOfRangeException(nameof(Note),
                    $"La note doit être comprise entre {NoteMoyenneMinimum} et {NoteMoyenneMaximum}.");
            }
            Note = pNoteObtenue;
        }

        public int CompareTo(NoteFilm? other)
        {
            if (ReferenceEquals(this, other))
            {
                return 0;
            }

            if (ReferenceEquals(null, other))
            {
                return 1;
            }

            int idFilmComparison = IdFilm.CompareTo(other.IdFilm);
            if (idFilmComparison != 0)
            {
                return idFilmComparison; 
            }

            return IdUtilisateur.CompareTo(other.IdUtilisateur);
        }
    }
}
