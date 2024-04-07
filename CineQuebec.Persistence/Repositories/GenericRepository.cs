using System.Collections.Immutable;
using System.Linq.Expressions;
using CineQuebec.Application.Interfaces.Repositories;
using CineQuebec.Domain.Entities.Abstract;
using CineQuebec.Domain.Interfaces.Entities;
using CineQuebec.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CineQuebec.Persistence.Repositories;

public class GenericRepository<TIEntite> : IRepository<TIEntite> where TIEntite : class, IEntite
{
	private readonly IApplicationDbContext _context;
	private readonly DbSet<TIEntite> _dbSet;

	public GenericRepository(IApplicationDbContext context)
	{
		_context = context;
		_dbSet = _context.Set<TIEntite>();
	}

    public async Task<bool> ExisteAsync(Expression<Func<TIEntite, bool>> filtre)
    {
        return await _dbSet.AnyAsync(filtre);
    }

    public async Task<TIEntite> AjouterAsync(TIEntite entite)
	{
		var res = await _dbSet.AddAsync(entite).AsTask();
		return res.Entity as TIEntite;
	}

	public TIEntite Modifier(TIEntite entite)
	{
		_dbSet.Attach(entite);
		_context.Entry(entite).State = EntityState.Modified;
		return entite;
	}

	public async Task<TIEntite?> ObtenirParIdAsync(Guid id)
	{
		return await _dbSet.FindAsync(id).AsTask();
	}

    public async Task<IEnumerable<TIEntite>> ObtenirParIdsAsync(IEnumerable<Guid> ids)
    {
        return await _dbSet.Where(e => ids.Contains(e.Id)).ToArrayAsync();
    }

	public async Task<IEnumerable<TIEntite>> ObtenirTousAsync(Expression<Func<TIEntite, bool>>? filtre = null,
		Func<IQueryable<TIEntite>, IOrderedQueryable<TIEntite>>? trierPar = null)
	{
		IQueryable<TIEntite> query = _dbSet;

		if (filtre != null)
		{
			query = query.Where(filtre);
		}

		return trierPar is not null ? await trierPar(query).ToArrayAsync() : await query.ToArrayAsync();
	}

	public void Supprimer(TIEntite entite)
	{
		if (_context.Entry(entite).State == EntityState.Detached)
		{
			_dbSet.Attach(entite);
		}

		_dbSet.Remove(entite);
	}

	public void Supprimer(Guid id)
	{
		if (_dbSet.Find(id) is { } entite)
		{
			Supprimer(entite);
		}
	}
}