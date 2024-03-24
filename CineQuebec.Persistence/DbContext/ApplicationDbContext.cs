using System.Reflection;

using CineQuebec.Application.Interfaces;
using CineQuebec.Domain.Entities.Abstract;
using CineQuebec.Domain.Entities.Utilisateurs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MongoDB.EntityFrameworkCore.Extensions;

namespace CineQuebec.Persistence.DbContext;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
	: Microsoft.EntityFrameworkCore.DbContext(options), IApplicationDbContext
{
	public new DbSet<TEntite> Set<TEntite>() where TEntite : Entite
	{
		return base.Set<TEntite>();
	}

	public new EntityEntry<TEntite> Entry<TEntite>(TEntite entite) where TEntite : Entite
	{
		return base.Entry(entite);
	}

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);

		builder.Entity<Utilisateur>().Property(e => e.Roles).HasConversion<string[]>();
		builder.Entity<Utilisateur>().ToCollection("utilisateurs");

		builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
	}
}