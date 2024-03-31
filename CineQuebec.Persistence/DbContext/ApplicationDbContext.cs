using System.Reflection;
using CineQuebec.Domain.Entities.Abstract;
using CineQuebec.Domain.Entities.Films;
using CineQuebec.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MongoDB.EntityFrameworkCore.Extensions;

namespace CineQuebec.Persistence.DbContext;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
	: Microsoft.EntityFrameworkCore.DbContext(options), IApplicationDbContext
{
	public new EntityEntry<TEntite> Entry<TEntite>(TEntite entite) where TEntite : Entite
	{
		return base.Entry(entite);
	}

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);

		builder.Entity<Film>().ToCollection("films")
			.HasMany(f => f.Acteurs);
		builder.Entity<Acteur>().ToCollection("acteurs");
		builder.Entity<Realisateur>().ToCollection("realisateurs");
		builder.Entity<CategorieFilm>().ToCollection("categories");

		builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
	}

	public new DbSet<TEntite> Set<TEntite>() where TEntite : Entite
	{
		return base.Set<TEntite>();
	}
}