using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services.Films;
using CineQuebec.Application.Interfaces.Services.Identity;
using CineQuebec.Application.Interfaces.Services.Preferences;
using CineQuebec.Application.Records.Films;
using CineQuebec.Domain.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Films;

namespace CineQuebec.Application.Services.Preferences;

public class NoteFilmCreationService(
    IUnitOfWorkFactory unitOfWorkFactory,
    IFilmQueryService filmQueryService,
    IUtilisateurAuthenticationService utilisateurAuthenticationService)
    : INoteFilmCreationService
{
    public async Task<float> NoterFilm(Guid pIdFilm, byte pNouvelleNote)
    {
        Guid idUtilisateur = utilisateurAuthenticationService.ObtenirIdUtilisateurConnecte();

        using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();

        await ValiderUtilisateurAVisionneFilm(pIdFilm);

        IFilm film = await unitOfWork.FilmRepository.ObtenirParIdAsync(pIdFilm) ??
                     throw new KeyNotFoundException($"Le film {pIdFilm} est introuvable");

        INoteFilm? noteActuelle =
            await unitOfWork.NoteFilmRepository.ObtenirAsync(n =>
                n.IdUtilisateur == idUtilisateur && n.IdFilm == pIdFilm);

        return noteActuelle is not null
            ? await ModifierNoteExistante(unitOfWork, film, noteActuelle, pNouvelleNote)
            : await CreerNouvelleNote(unitOfWork, film, idUtilisateur, pNouvelleNote);
    }

    private static async Task<float> ModifierNoteExistante(IUnitOfWork unitOfWork, IFilm film, INoteFilm noteActuelle,
        byte pNouvelleNote)
    {
        film.ModifierNote(noteActuelle.Note, pNouvelleNote);
        noteActuelle.SetNote(pNouvelleNote);
        unitOfWork.NoteFilmRepository.Modifier(noteActuelle);
        unitOfWork.FilmRepository.Modifier(film);
        await unitOfWork.SauvegarderAsync();
        return film.NoteMoyenne ?? 0;
    }

    private static async Task<float> CreerNouvelleNote(IUnitOfWork unitOfWork, IFilm film, Guid idUtilisateur,
        byte pNouvelleNote)
    {
        NoteFilm nouvelleNote = new(idUtilisateur, film.Id, pNouvelleNote);
        _ = await unitOfWork.NoteFilmRepository.AjouterAsync(nouvelleNote);
        film.AjouterNote(pNouvelleNote);
        unitOfWork.FilmRepository.Modifier(film);
        await unitOfWork.SauvegarderAsync();
        return film.NoteMoyenne ?? 0;
    }

    private async Task ValiderUtilisateurAVisionneFilm(Guid idFilm)
    {
        IEnumerable<FilmDto> filmsVisionnes = await filmQueryService.ObtenirFilmsAssistesParUtilisateur();

        if (filmsVisionnes.All(fv => fv.Id != idFilm))
        {
            throw new InvalidOperationException(
                "Impossible de noter ce film sans avoir assisté à l'une de ses projection.");
        }
    }
}