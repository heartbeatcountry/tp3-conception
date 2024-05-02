using System.Security;
using System.Security.Claims;

using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services;
using CineQuebec.Application.Services.Abstract;
using CineQuebec.Domain.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Films;

namespace CineQuebec.Application.Services
{
    public class NoteFilmCreationService(IUnitOfWorkFactory unitOfWorkFactory, IUtilisateurAuthenticationService utilisateurAuthenticationService) : ServiceAvecValidation, INoteFilmCreationService
    {

        public async Task<Guid> NoterFilm(Guid pIdFilm, byte pNote)
        {
            using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();

            Guid idUtilisateur = utilisateurAuthenticationService.ObtenirIdUtilisateur();


            IEnumerable<Exception> exceptions = await EffectuerValidations(unitOfWork, idUtilisateur, pIdFilm, pNote);

            NoteFilm notefilm = new(idUtilisateur!, pIdFilm, pNote);
            INoteFilm notefilmCreee = await unitOfWork.NoteFilmRepository.AjouterAsync(notefilm);

            await unitOfWork.SauvegarderAsync();

            return notefilmCreee.Id;
        }


        private static async Task EffectuerValidations(IUnitOfWork unitOfWork, Guid? pIdUtilisateur, Guid pIdFilm, byte pNote)
        {
            LeverAggregateExceptionAuBesoin(
                await ValiderFilmExiste(unitOfWork, pIdFilm),
                await ValiderNoteFilmParUtilisateurUnique(unitOfWork, pIdFilm, pIdUtilisateur)
            );


            List<Exception> exceptions = [];

            exceptions.AddRange();
            //exceptions.AddRange(await ValiderUtilisateurExiste(unitOfWork, pIdUtilisateur));
            exceptions.AddRange();
            exceptions.AddRange(ValiderNoteFilmValide(pNote));

            return exceptions;
        }

        private static IEnumerable<Exception> ValiderNoteFilmValide(ushort pNote)
        {
            List<Exception> exceptions = [];

            if (pNote >= 10 && pNote <= 0)
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

        //TODO
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