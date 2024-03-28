using System.Collections.Immutable;
using CineQuebec.Domain.Entities.Abstract;
using CineQuebec.Domain.Interfaces.Entities.Films;

namespace CineQuebec.Domain.Entities.Films;

public class Realisateur : Personne, IRealisateur
{
	private readonly HashSet<IFilm> _realiseFilms = [];

	public Realisateur(string prenom, string nom) : base(prenom, nom)
	{
	}

	public ImmutableArray<IFilm> RealiseFilms => _realiseFilms.ToImmutableArray();

	public bool AjouterFilm(IFilm film)
	{
		return _realiseFilms.Add(film);
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
		return _realiseFilms.Remove(film);
	}
}