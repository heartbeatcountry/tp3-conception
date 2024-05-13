using System.Xml.Linq;

using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services.Films;
using CineQuebec.Application.Interfaces.Services.Identity;
using CineQuebec.Application.Interfaces.Services.Preferences;
using CineQuebec.Application.Services.Abstract;
using CineQuebec.Domain.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Utilisateur;

namespace CineQuebec.Application.Services.Preferences;

public class NoteFilmCreationService(
    IUnitOfWorkFactory unitOfWorkFactory,
    IUtilisateurAuthenticationService utilisateurAuthenticationService)
    : ServiceAvecValidation, INoteFilmCreationService
{
    public async Task<Guid> NoterFilm(Guid pIdFilm, byte pNouvelleNote)
    {
        using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();

        Guid idUtilisateur = utilisateurAuthenticationService.ObtenirIdUtilisateurConnecte();
        EffectuerValidations(unitOfWork, pIdFilm, pNouvelleNote);

        IFilm film = await unitOfWork.FilmRepository.ObtenirParIdAsync(pIdFilm);


        if (await unitOfWork.NoteFilmRepository.ExisteAsync(n => n.IdUtilisateur == idUtilisateur && n.IdFilm == pIdFilm))
        {
            INoteFilm noteActuelle = await unitOfWork.NoteFilmRepository.ObtenirAsync(n => n.IdUtilisateur == idUtilisateur && n.IdFilm == pIdFilm);
            film.ModifierNote(noteActuelle.Note, pNouvelleNote);
            noteActuelle.SetNote(pNouvelleNote);
            await unitOfWork.SauvegarderAsync();
            return noteActuelle.Id;
        }
        else
        {
            NoteFilm notefilm = new(idUtilisateur, pIdFilm, pNouvelleNote);
            INoteFilm notefilmCreee = await unitOfWork.NoteFilmRepository.AjouterAsync(notefilm);
           film.AjouterNote(pNouvelleNote);
            await unitOfWork.SauvegarderAsync();
            return notefilmCreee.Id;
        }

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