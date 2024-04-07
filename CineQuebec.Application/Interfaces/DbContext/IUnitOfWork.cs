using CineQuebec.Application.Interfaces.Repositories;
using CineQuebec.Domain.Entities.Films;
using CineQuebec.Domain.Entities.Projections;
using CineQuebec.Domain.Interfaces.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Projections;

namespace CineQuebec.Application.Interfaces.DbContext;

public interface IUnitOfWork : IDisposable
{
	IRepository<IActeur> ActeurRepository { get; }
	IRepository<ICategorieFilm> CategorieFilmRepository { get; }
	IRepository<IFilm> FilmRepository { get; }

	IRepository<IRealisateur> RealisateurRepository { get; }

	IRepository<IProjection> ProjectionRepository { get; }
	IRepository<ISalle> SalleRepository { get; }
	public Task<int> SauvegarderAsync(CancellationToken? cancellationToken = null);
}