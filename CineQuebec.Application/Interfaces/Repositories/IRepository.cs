using System.Collections.Immutable;
using System.Linq.Expressions;
using CineQuebec.Domain.Entities.Abstract;

namespace CineQuebec.Application.Interfaces.Repositories;

public interface IRepository<TEntite> where TEntite : Entite
{
	public Task<TEntite?> ObtenirParIdAsync(Guid id);

    public Task<IEnumerable<TEntite>> ObtenirParIdsAsync(IEnumerable<Guid> ids);

	public Task<IEnumerable<TEntite>> ObtenirTousAsync(Expression<Func<TEntite, bool>>? filtre = null,
		Func<IQueryable<TEntite>, IOrderedQueryable<TEntite>>? trierPar = null);

	public Task<TEntite> AjouterAsync(TEntite entite);
	public TEntite Modifier(TEntite entite);
	public void Supprimer(TEntite entite);
	public void Supprimer(Guid id);
}