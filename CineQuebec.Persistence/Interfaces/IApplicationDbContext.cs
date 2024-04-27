using CineQuebec.Domain.Interfaces.Entities;
using CineQuebec.Domain.Interfaces.Entities.Abstract;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CineQuebec.Persistence.Interfaces;

public interface IApplicationDbContext
{
    public DbSet<TIEntite> Set<TIEntite>() where TIEntite : class, IEntite;
    public EntityEntry<TIEntite> Entry<TIEntite>(TIEntite entite) where TIEntite : class, IEntite;
}