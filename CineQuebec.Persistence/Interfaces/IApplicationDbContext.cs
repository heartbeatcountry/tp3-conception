using CineQuebec.Domain.Entities.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CineQuebec.Persistence.Interfaces;

public interface IApplicationDbContext
{
	public DbSet<TEntite> Set<TEntite>() where TEntite : Entite;
	public EntityEntry<TEntite> Entry<TEntite>(TEntite entite) where TEntite : Entite;
}