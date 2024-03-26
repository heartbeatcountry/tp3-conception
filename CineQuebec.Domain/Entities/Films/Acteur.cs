using System.Collections.Immutable;
using CineQuebec.Domain.Entities.Abstract;
using CineQuebec.Domain.Interfaces.Entities.Films;

namespace CineQuebec.Domain.Entities.Films;

public class Acteur(string prenom, string nom) : Personne(prenom, nom), IActeur
{
	private readonly HashSet<IFilm> _joueDansFilms = [];

	public ImmutableArray<IFilm> JoueDansFilms => _joueDansFilms.ToImmutableArray();

	public bool AjouterFilm(IFilm film)
	{
		return _joueDansFilms.Add(film);
	}

	public void AjouterFilms(IEnumerable<IFilm> films)
	{
		foreach (var film in films)
		{
			AjouterFilm(film);
		}
	}

	public bool RetirerFilm(IFilm film)
	{
		return _joueDansFilms.Remove(film);
	}
}