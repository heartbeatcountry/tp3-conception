using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services;
using CineQuebec.Domain.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Films;

namespace CineQuebec.Application.Services
{
    public class NoteFilmCreationService(IUnitOfWorkFactory unitOfWorkFactory) : INoteFillmCreationService
    {

        public async Task<Guid> CreerNoteFilm(Guid pIdUtilisateur, Guid pIdFilm, ushort pNote)
        {
            using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();

            IEnumerable<Exception> exceptions = await EffectuerValidations(unitOfWork, pIdUtilisateur, pIdFilm, pNote);

            if (exceptions.ToArray() is { Length: > 0 } innerExceptions)
            {
                throw new AggregateException("Des erreurs se sont produites lors de la validation des données.",
                    innerExceptions);
            }

            NoteFilm notefilm = new(pIdUtilisateur, pIdFilm, pNote);
            INoteFilm notefilmCreee = await unitOfWork.NoteFilmRepository.AjouterAsync(notefilm);

            await unitOfWork.SauvegarderAsync();

            return notefilmCreee.Id;
        }


        private static async Task<IEnumerable<Exception>> EffectuerValidations(IUnitOfWork unitOfWork, Guid pIdUtilisateur, Guid pIdFilm, ushort pNote)
        {
            List<Exception> exceptions = [];

            exceptions.AddRange(await ValiderFilmExiste(unitOfWork, pIdFilm));
            //exceptions.AddRange(await ValiderUtilisateurExiste(unitOfWork, pIdUtilisateur));
            exceptions.AddRange(await ValiderNoteFilmParUtilisateurUnique(unitOfWork, pIdFilm, pIdUtilisateur));
            exceptions.AddRange(ValiderNoteFilmValide(pNote));

            return exceptions;
        }

        private static IEnumerable<Exception> ValiderNoteFilmValide(ushort pNote)
        {
            List<Exception> exceptions = [];

            if (pNote <= 10 && pNote >= 0)
            {
                exceptions.Add(new ArgumentOutOfRangeException(nameof(pNote),
                    "La notefilm doit être entre 0 et 10."));
            }

            return exceptions;
        }


        private static async Task<IEnumerable<Exception>> ValiderFilmExiste(IUnitOfWork unitOfWork, Guid pIdFilm)
        {
            List<Exception> exceptions = [];

            if (await unitOfWork.FilmRepository.ObtenirParIdAsync(pIdFilm) is null)
            {
                exceptions.Add(new ArgumentException($"Le film avec l'identifiant {pIdFilm} n'existe pas.",
                    nameof(pIdFilm)));
            }

            return exceptions;
        }


        private static async Task<IEnumerable<Exception>> ValiderNoteFilmParUtilisateurUnique(IUnitOfWork unitOfWork, Guid pIdFilm,
            Guid pIdUtilisateur)
        {
            List<Exception> exceptions = [];

            if (await unitOfWork.NoteFilmRepository.ExisteAsync(f =>
                    f.IdFilm == pIdFilm && f.IdUtilisateur == pIdUtilisateur ))
            {
                exceptions.Add(new ArgumentException(
                    "Une notefilm pour un film par cet utilisateur existe déjà.",
                    nameof(pIdFilm)));
            }

            return exceptions;
        }

        //private static async Task<IEnumerable<Exception>> ValiderUtilisateurExiste(IUnitOfWork unitOfWork, Guid pIdUtilisateur)
        //{
        //    List<Exception> exceptions = [];

        //    if (await unitOfWork.UtilisateurRepository.ObtenirParIdAsync(pIdUtilisateur) is null)
        //    {
        //        exceptions.Add(new ArgumentException($"L'utilisateur avec l'identifiant {pIdUtilisateur} n'existe pas.",
        //            nameof(pIdUtilisateur))));
        //    }

        //    return exceptions;
        //}
    } 
}