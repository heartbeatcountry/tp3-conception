using System.Linq.Expressions;

using CineQuebec.Domain.Interfaces.Entities.Abstract;

namespace CineQuebec.Application.Interfaces.Repositories;

public interface IRepository<TIEntite> where TIEntite : IEntite
{
    public Task<TIEntite?> ObtenirParIdAsync(Guid id);

    public Task<IEnumerable<TIEntite>> ObtenirParIdsAsync(IEnumerable<Guid> ids);

    public Task<IEnumerable<TIEntite>> ObtenirTousAsync(Expression<Func<TIEntite, bool>>? filtre = null,
        Func<IQueryable<TIEntite>, IOrderedQueryable<TIEntite>>? trierPar = null);

    public Task<int> CompterAsync(Expression<Func<TIEntite, bool>>? filtre = null);

    public Task<bool> ExisteAsync(Expression<Func<TIEntite, bool>> filtre);
    public Task<bool> ExisteAsync(Guid id);

    public Task<TIEntite?> ObtenirAsync(Expression<Func<TIEntite, bool>> filtre);

    public Task<TIEntite> AjouterAsync(TIEntite entite);
    public TIEntite Modifier(TIEntite entite);
    public void Supprimer(TIEntite entite);
    public void Supprimer(Guid id);
}