using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services.Identity;
using CineQuebec.Application.Interfaces.Services.Preferences;
using CineQuebec.Domain.Interfaces.Entities.Films;

namespace CineQuebec.Application.Services.Preferences;

public class NoteFilmQueryService(
    IUnitOfWorkFactory unitOfWorkFactory,
    IUtilisateurAuthenticationService utilisateurAuthenticationService)
    : INoteFilmQueryService
{
    public async Task<byte?> ObtenirMaNotePourFilm(Guid pIdFilm)
    {
        Guid idUtilisateur = utilisateurAuthenticationService.ObtenirIdUtilisateurConnecte();

        using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();

        INoteFilm? noteActuelle =
            await unitOfWork.NoteFilmRepository.ObtenirAsync(n =>
                n.IdUtilisateur == idUtilisateur && n.IdFilm == pIdFilm);

        return noteActuelle?.Note;
    }
}