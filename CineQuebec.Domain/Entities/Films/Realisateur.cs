using System.Collections.Immutable;
using CineQuebec.Domain.Entities.Abstract;
using CineQuebec.Domain.Interfaces.Entities.Films;

namespace CineQuebec.Domain.Entities.Films;

public class Realisateur(string prenom, string nom) : Personne(prenom, nom), IRealisateur
{
	private readonly HashSet<IFilm> _realiseFilms = [];

	private Realisateur(Guid id, string prenom, string nom) : this(prenom, nom)
	{
		SetId(id);
	}

	public ImmutableArray<IFilm> RealiseFilms => _realiseFilms.ToImmutableArray();

	public bool AjouterFilm(IFilm film)
	{
		return _realiseFilms.Add(film);
	}

	public bool RetirerFilm(IFilm film)
	{
		return _realiseFilms.Remove(film);
	}
}