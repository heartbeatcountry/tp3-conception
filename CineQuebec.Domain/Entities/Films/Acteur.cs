using System.Collections.Immutable;
using CineQuebec.Domain.Entities.Abstract;

namespace CineQuebec.Domain.Entities.Films;

public class Acteur(string prenom, string nom) : Personne(prenom, nom)
{
	private readonly HashSet<Film> _joueDansFilms = [];

	public ImmutableArray<Film> JoueDansFilms => _joueDansFilms.ToImmutableArray();

	private bool AjouterFilm(Film film)
	{
		return _joueDansFilms.Add(film);
	}

	private void AjouterFilms(IEnumerable<Film> films)
	{
		foreach (var film in films)
		{
			AjouterFilm(film);
		}
	}

	private bool RetirerFilm(Film film)
	{
		return _joueDansFilms.Remove(film);
	}
}