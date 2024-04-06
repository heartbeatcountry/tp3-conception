using CineQuebec.Application.Interfaces.Repositories;
using CineQuebec.Domain.Entities.Films;
using CineQuebec.Domain.Entities.Projections;

namespace CineQuebec.Application.Interfaces.DbContext;

public interface IUnitOfWork : IDisposable
{
	IRepository<Acteur> ActeurRepository { get; }
	IRepository<CategorieFilm> CategorieFilmRepository { get; }
	IRepository<Film> FilmRepository { get; }

	IRepository<Realisateur> RealisateurRepository { get; }

	IRepository<Projection> ProjectionRepository { get; }
	IRepository<Salle> SalleRepository { get; }
	public int Sauvegarder();
	public Task<int> SauvegarderAsync(CancellationToken? cancellationToken = null);
}