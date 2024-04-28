using System.Linq.Expressions;

using CineQuebec.Application.Interfaces.Repositories;
using CineQuebec.Domain.Entities.Abstract;
using CineQuebec.Domain.Interfaces.Entities.Abstract;
using CineQuebec.Persistence.Interfaces;
using CineQuebec.Persistence.Lib;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CineQuebec.Persistence.Repositories;

public class GenericRepository<TEntite, TIEntite> : IRepository<TIEntite>
    where TEntite : Entite where TIEntite : class, IEntite
{
    private readonly IApplicationDbContext _context;
    private readonly DbSet<TEntite> _dbSet;

    public GenericRepository(IApplicationDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntite>();
    }

    public Task<TIEntite?> ObtenirAsync(Expression<Func<TIEntite, bool>> filtre)
    {
        Expression<Func<TEntite, bool>> filtreExp = filtre.ReplaceTypeParameter<TIEntite, TEntite, bool>();
        return (_dbSet.FirstOrDefaultAsync(filtreExp) as Task<TIEntite?>)!;
    }

    public async Task<TIEntite> AjouterAsync(TIEntite entite)
    {
        EntityEntry<TEntite> res = await _dbSet.AddAsync((entite as TEntite)!).AsTask();
        return (res.Entity as TIEntite)!;
    }

    public async Task<bool> ExisteAsync(Expression<Func<TIEntite, bool>> filtre)
    {
        Expression<Func<TEntite, bool>> filtreExp = filtre.ReplaceTypeParameter<TIEntite, TEntite, bool>();
        return await _dbSet.AnyAsync(filtreExp);
    }

    public TIEntite Modifier(TIEntite entite)
    {
        _dbSet.Attach((entite as TEntite)!);
        _context.Entry(entite).State = EntityState.Modified;
        return entite;
    }

    public async Task<TIEntite?> ObtenirParIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id).AsTask() as TIEntite;
    }

    public async Task<IEnumerable<TIEntite>> ObtenirParIdsAsync(IEnumerable<Guid> ids)
    {
        return (await _dbSet.Where(e => ids.Contains(e.Id)).ToArrayAsync() as IEnumerable<TIEntite>)!;
    }

    public async Task<IEnumerable<TIEntite>> ObtenirTousAsync(Expression<Func<TIEntite, bool>>? filtre = null,
        Func<IQueryable<TIEntite>, IOrderedQueryable<TIEntite>>? trierPar = null)
    {
        IQueryable<TEntite> query = _dbSet;

        if (filtre != null)
        {
            Expression<Func<TEntite, bool>> filtreExp = filtre.ReplaceTypeParameter<TIEntite, TEntite, bool>();
            query = query.Where(filtreExp);
        }

        return (trierPar is not null
            ? await trierPar((query as IQueryable<TIEntite>)!).ToArrayAsync()
            : await query.ToArrayAsync() as IEnumerable<TIEntite>)!;
    }

    public void Supprimer(TIEntite entite)
    {
        if (_context.Entry(entite).State == EntityState.Detached)
        {
            _dbSet.Attach((entite as TEntite)!);
        }

        _dbSet.Remove((entite as TEntite)!);
    }

    public void Supprimer(Guid id)
    {
        if (_dbSet.Find(id) is TIEntite entite)
        {
            Supprimer(entite);
        }
    }
}