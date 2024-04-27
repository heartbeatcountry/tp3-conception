using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Repositories;
using CineQuebec.Domain.Entities.Films;
using CineQuebec.Domain.Entities.Projections;
using CineQuebec.Domain.Entities.Utilisateurs;
using CineQuebec.Domain.Interfaces.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Projections;
using CineQuebec.Domain.Interfaces.Entities.Utilisateur;
using CineQuebec.Persistence.Interfaces;
using CineQuebec.Persistence.Repositories;

namespace CineQuebec.Persistence.DbContext;

public sealed class UnitOfWork(IApplicationDbContextFactory applicationDbContextFactory) : IUnitOfWork
{
    private readonly ApplicationDbContext _context = applicationDbContextFactory.Create();

    private IRepository<IActeur>? _acteurRepository;

    private IRepository<ICategorieFilm>? _categorieFilmRepository;

    private bool _disposed;

    private IRepository<IFilm>? _filmRepository;

    private IRepository<IProjection>? _projectionRepository;
    private IRepository<IRealisateur>? _realisateurRepository;
    private IRepository<ISalle>? _salleRepository;

    private IRepository<IUtilisateur>? _utilisateurRepository;

    public IRepository<ISalle> SalleRepository =>
        _salleRepository ??= new GenericRepository<Salle, ISalle>(_context);

    public IRepository<IActeur> ActeurRepository =>
        _acteurRepository ??= new GenericRepository<Acteur, IActeur>(_context);

    public IRepository<ICategorieFilm> CategorieFilmRepository =>
        _categorieFilmRepository ??= new GenericRepository<CategorieFilm, ICategorieFilm>(_context);

    public IRepository<IFilm> FilmRepository =>
        _filmRepository ??= new GenericRepository<Film, IFilm>(_context);

    public IRepository<IProjection> ProjectionRepository =>
        _projectionRepository ??= new GenericRepository<Projection, IProjection>(_context);

    public IRepository<IRealisateur> RealisateurRepository =>
        _realisateurRepository ??= new GenericRepository<Realisateur, IRealisateur>(_context);

    public IRepository<IUtilisateur> UtilisateurRepository =>
        _utilisateurRepository ??= new GenericRepository<Utilisateur, IUtilisateur>(_context);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public async Task<int> SauvegarderAsync(CancellationToken? cancellationToken = null)
    {
        return await _context.SaveChangesAsync(cancellationToken ?? CancellationToken.None);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _context.Dispose();
        }

        _disposed = true;
    }

    ~UnitOfWork()
    {
        Dispose(false);
    }
}