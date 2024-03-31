using System.Collections.Immutable;
using CineQuebec.Domain.Entities.Abstract;
using CineQuebec.Domain.Interfaces.Entities.Films;

namespace CineQuebec.Domain.Entities.Films;

public class Acteur(string prenom, string nom) : Personne(prenom, nom), IActeur
{
	private readonly HashSet<IFilm> _joueDansFilms = [];

	private Acteur(Guid id, string prenom, string nom) : this(prenom, nom)
	{
		SetId(id);
	}

	public ImmutableArray<IFilm> JoueDansFilms => _joueDansFilms.ToImmutableArray();

	public bool AjouterFilm(IFilm film)
	{
		if (film.Id == Guid.Empty)
		{
			throw new ArgumentException("Le film doit avoir un identifiant unique.", nameof(film));
		}

		return _joueDansFilms.Add(film);
	}

	public bool RetirerFilm(IFilm film)
	{
		return _joueDansFilms.Remove(film);
	}
}