using System.Globalization;
using System.Reflection;
using CineQuebec.Domain.Entities.Abstract;
using CineQuebec.Domain.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities;
using CineQuebec.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MongoDB.EntityFrameworkCore.Extensions;

namespace CineQuebec.Persistence.DbContext;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
	: Microsoft.EntityFrameworkCore.DbContext(options), IApplicationDbContext
{
	private static readonly Func<Guid, string> GuidToStringConverter = guid => guid.ToString();
	private static readonly Func<string, Guid> StringToGuidConverter = Guid.Parse;

	private static readonly ValueConverter<IEnumerable<Guid>, string[]> GuidsToStringsConverter = new(
		guids => guids.Select(GuidToStringConverter).ToArray(),
		strings => strings.Select(StringToGuidConverter).ToHashSet());

	private static readonly ValueConverter<DateTime, string> DateOnlyToStringConverter = new(
		dateTime => DateOnly.FromDateTime(dateTime).ToString(CultureInfo.InvariantCulture),
		str => DateOnly.Parse(str).ToDateTime(TimeOnly.MinValue));

	public new EntityEntry<TEntite> Entry<TEntite>(TEntite entite) where TEntite : class, IEntite
	{
		return base.Entry(entite);
	}

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);

		var films = builder.Entity<Film>().ToCollection("films");
		var acteurs = builder.Entity<Acteur>().ToCollection("acteurs");
		var realisateurs = builder.Entity<Realisateur>().ToCollection("realisateurs");
		var categorieFilms = builder.Entity<CategorieFilm>().ToCollection("categories");

		films
			.Property(film => film.Id)
			.HasConversion<string>();

		films
			.Property(film => film.ActeursParId)
			.HasConversion(GuidsToStringsConverter)
			.UsePropertyAccessMode(PropertyAccessMode.Property);

		films
			.Property(film => film.RealisateursParId)
			.HasConversion(GuidsToStringsConverter)
			.UsePropertyAccessMode(PropertyAccessMode.Property);

		films
			.Property(film => film.DateSortieInternationale)
			.HasConversion(DateOnlyToStringConverter);

		acteurs
			.Property(film => film.Id)
			.HasConversion<string>();

		realisateurs
			.Property(film => film.Id)
			.HasConversion<string>();

		categorieFilms
			.Property(film => film.Id)
			.HasConversion<string>();

		builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
	}

	public new DbSet<TEntite> Set<TEntite>() where TEntite : class, IEntite
	{
		return base.Set<TEntite>();
	}
}