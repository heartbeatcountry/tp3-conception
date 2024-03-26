using CineQuebec.Domain.Entities.Films;
using CineQuebec.Domain.Entities.Projections;
using CineQuebec.Domain.Entities.Utilisateurs;
using CineQuebec.Domain.Interfaces;
using CineQuebec.Domain.Interfaces.Repositories;

namespace CineQuebec.Application.Interfaces.DbContext;

public interface IUnitOfWork : IDisposable
{
	IRepository<Acteur> ActeurRepository { get; }
	IRepository<CategorieFilm> CategorieFilmRepository { get; }
	IRepository<Film> FilmRepository { get; }
	IRepository<Realisateur> RealisateurRepository { get; }
	IRepository<Billet> BilletRepository { get; }
	IRepository<Projection> ProjectionRepository { get; }
	IRepository<Salle> SalleRepository { get; }
	IRepository<Utilisateur> UtilisateurRepository { get; }
	public int Sauvegarder();
	public Task<int> SauvegarderAsync(CancellationToken? cancellationToken = null);
}