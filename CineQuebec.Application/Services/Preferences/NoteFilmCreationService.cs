using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services.Identity;
using CineQuebec.Application.Interfaces.Services.Preferences;
using CineQuebec.Application.Services.Abstract;
using CineQuebec.Domain.Entities.Films;

namespace CineQuebec.Application.Services.Preferences;

public class NoteFilmCreationService(
    IUnitOfWorkFactory unitOfWorkFactory,
    IUtilisateurAuthenticationService utilisateurAuthenticationService)
    : ServiceAvecValidation, INoteFilmCreationService
{
    public async Task<Guid> NoterFilm(Guid pIdFilm, byte pNote)
    {
        using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();

        Guid idUtilisateur = utilisateurAuthenticationService.ObtenirIdUtilisateurConnecte();

        EffectuerValidations(unitOfWork, pIdFilm, pNote);

        // TODO: si l'utilisateur a déjà noté le film, mettre à jour la note existante
        //       sinon, créer une nouvelle note

        //NoteFilm notefilm = new(idUtilisateur, pIdFilm, pNote);
        //INoteFilm notefilmCreee = await unitOfWork.NoteFilmRepository.AjouterAsync(notefilm);

        //await unitOfWork.SauvegarderAsync();

        //return notefilmCreee.Id;

        throw new NotImplementedException();
    }


    private static void EffectuerValidations(IUnitOfWork unitOfWork, Guid pIdFilm, byte pNote)
    {
        LeverAggregateExceptionAuBesoin(
            ValiderNoteValeurMinMax(pNote),
            ValiderFilmExiste(unitOfWork, pIdFilm)
        );
    }

    private static IEnumerable<Exception> ValiderNoteValeurMinMax(byte pNote)
    {
        if (pNote is < NoteFilm.NoteMinimum or > NoteFilm.NoteMaximum)
        {
            yield return new ArgumentOutOfRangeException(nameof(pNote),
                $"La note doit être comprise entre {NoteFilm.NoteMinimum} et {NoteFilm.NoteMaximum}.");
        }
    }

    private static async IAsyncEnumerable<ArgumentException> ValiderFilmExiste(IUnitOfWork unitOfWork, Guid pIdFilm)
    {
        if (await unitOfWork.FilmRepository.ObtenirParIdAsync(pIdFilm) is null)
        {
            yield return new ArgumentException($"Le film avec l'identifiant {pIdFilm} n'existe pas.",
                nameof(pIdFilm));
        }
    }
}