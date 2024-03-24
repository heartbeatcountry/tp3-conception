using CineQuebec.Application.Interfaces;
using CineQuebec.Domain.Entities.Films;
using CineQuebec.Domain.Entities.Projections;
using CineQuebec.Domain.Entities.Utilisateurs;
using CineQuebec.Domain.Interfaces;
using CineQuebec.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace CineQuebec.Persistence.DbContext;

public class UnitOfWork(IMongoDatabase database) : IUnitOfWork
{
	private readonly ApplicationDbContext _context = new(new DbContextOptionsBuilder<ApplicationDbContext>()
		.UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName)
		.UseLazyLoadingProxies()
		.Options);

	private IRepository<Acteur>? _acteurRepository;
	private IRepository<Billet>? _billetRepository;
	private IRepository<CategorieFilm>? _categorieFilmRepository;

	private bool _disposed;
	private IRepository<Film>? _filmRepository;
	private IRepository<Projection>? _projectionRepository;
	private IRepository<Realisateur>? _realisateurRepository;
	private IRepository<Salle>? _salleRepository;
	private IRepository<Utilisateur>? _utilisateurRepository;

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	public IRepository<Salle> SalleRepository =>
		_salleRepository ??= new GenericRepository<Salle>(_context);

	public IRepository<Acteur> ActeurRepository =>
		_acteurRepository ??= new GenericRepository<Acteur>(_context);

	public IRepository<Billet> BilletRepository =>
		_billetRepository ??= new GenericRepository<Billet>(_context);

	public IRepository<CategorieFilm> CategorieFilmRepository =>
		_categorieFilmRepository ??= new GenericRepository<CategorieFilm>(_context);

	public IRepository<Film> FilmRepository =>
		_filmRepository ??= new GenericRepository<Film>(_context);

	public IRepository<Projection> ProjectionRepository =>
		_projectionRepository ??= new GenericRepository<Projection>(_context);

	public IRepository<Realisateur> RealisateurRepository =>
		_realisateurRepository ??= new GenericRepository<Realisateur>(_context);

	public IRepository<Utilisateur> UtilisateurRepository =>
		_utilisateurRepository ??= new GenericRepository<Utilisateur>(_context);

	public int Sauvegarder()
	{
		return _context.SaveChanges();
	}

	public async Task<int> SauvegarderAsync(CancellationToken? cancellationToken = null)
	{
		return await _context.SaveChangesAsync(cancellationToken ?? CancellationToken.None);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!_disposed && disposing)
		{
			_context.Dispose();
		}

		_disposed = true;
	}
}