using CineQuebec.Application.Interfaces.Repositories;
using CineQuebec.Domain.Interfaces.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Projections;
using CineQuebec.Domain.Interfaces.Entities.Utilisateur;

namespace CineQuebec.Application.Interfaces.DbContext;

public interface IUnitOfWork : IDisposable
{
    IRepository<IActeur> ActeurRepository { get; }
    IRepository<ICategorieFilm> CategorieFilmRepository { get; }
    IRepository<IFilm> FilmRepository { get; }
    IRepository<IRealisateur> RealisateurRepository { get; }
    IRepository<IProjection> ProjectionRepository { get; }
    IRepository<INoteFilm> NoteFilmRepository { get; }
    IRepository<ISalle> SalleRepository { get; }
    IRepository<IUtilisateur> UtilisateurRepository { get; }
    IRepository<IBillet> BilletRepository { get; }

    public Task<int> SauvegarderAsync(CancellationToken? cancellationToken = null);
}