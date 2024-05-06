using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services.Films;
using CineQuebec.Application.Records.Films;
using CineQuebec.Domain.Interfaces.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Projections;

namespace CineQuebec.Application.Services.Films;

public class FilmQueryService(IUnitOfWorkFactory unitOfWorkFactory) : IFilmQueryService
{
    public async Task<FilmDto?> ObtenirDetailsFilmParId(Guid id)
    {
        using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();
        IFilm? film = await unitOfWork.FilmRepository.ObtenirParIdAsync(id);

        if (film is null)
        {
            return null;
        }

        ICategorieFilm? categorie = await unitOfWork.CategorieFilmRepository.ObtenirParIdAsync(film.IdCategorie);
        IEnumerable<IRealisateur> realisateurs =
            (await unitOfWork.RealisateurRepository.ObtenirParIdsAsync(film.RealisateursParId))
            .OrderBy(realisateur => realisateur.Prenom).ThenBy(realisateur => realisateur.Nom);
        IEnumerable<IActeur> acteurs = (await unitOfWork.ActeurRepository.ObtenirParIdsAsync(film.ActeursParId))
            .OrderBy(acteur => acteur.Prenom).ThenBy(realisateur => realisateur.Nom);

        FilmDto filmDto = film.VersDto(categorie?.VersDto(), realisateurs.Select(r => r.VersDto()),
            acteurs.Select(a => a.VersDto()));

        return filmDto;
    }

    public async Task<IEnumerable<FilmDto>> ObtenirTous()
    {
        using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();
        IEnumerable<IFilm> films = await unitOfWork.FilmRepository.ObtenirTousAsync();
        return films.Select(f => f.VersDto(null, [], [])).OrderBy(film => film.Titre);
    }

    public async Task<IEnumerable<FilmDto>> ObtenirTousAlAffiche()
    {
        using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();
        IEnumerable<IProjection> projections =
            await unitOfWork.ProjectionRepository.ObtenirTousAsync(pr => pr.DateHeure >= DateTime.Now);
        IEnumerable<IFilm> films =
            await unitOfWork.FilmRepository.ObtenirParIdsAsync(projections.Select(p => p.IdFilm).Distinct());
        return films.Select(f => f.VersDto(null, [], [])).OrderBy(film => film.Titre)
            .ThenBy(film => film.DateSortieInternationale);
    }


    public async Task<IEnumerable<FilmDto>> ObtenirTousAssiste()
    {
        //TODO Doit-on cr�er l'entit� "billet" pour pouvoir obtenir les films auxquels un utilisateur a assist�?

        throw new NotImplementedException();
    }
}