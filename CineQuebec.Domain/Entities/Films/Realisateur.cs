using System.Collections.Immutable;
using CineQuebec.Domain.Entities.Abstract;

namespace CineQuebec.Domain.Entities.Films;

public class Realisateur : Personne
{
	private readonly HashSet<Film> _realiseFilms = [];

	public Realisateur(string prenom, string nom) : base(prenom, nom)
	{
	}

	public ImmutableArray<Film> RealiseFilms => _realiseFilms.ToImmutableArray();

	private void AjouterFilm(Film film)
	{
		_realiseFilms.Add(film);
	}

	private void AjouterFilms(IEnumerable<Film> films)
	{
		foreach (var film in films)
		{
			AjouterFilm(film);
		}
	}

	private void RetirerFilm(Film film)
	{
		_realiseFilms.Remove(film);
	}
}