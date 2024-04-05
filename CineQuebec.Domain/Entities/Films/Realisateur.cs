using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using CineQuebec.Domain.Entities.Abstract;
using CineQuebec.Domain.Interfaces.Entities.Films;

namespace CineQuebec.Domain.Entities.Films;

public class Realisateur(string prenom, string nom) : Personne(prenom, nom), IRealisateur
{
	private readonly HashSet<Guid> _realiseFilms = [];

	[SuppressMessage("ReSharper", "UnusedMember.Local")]
	private Realisateur(Guid id, string prenom, string nom) : this(prenom, nom)
	{
		// Constructeur avec identifiant pour Entity Framework Core
		SetId(id);
	}

	public ImmutableArray<Guid> RealiseFilmsAvecId => _realiseFilms.ToImmutableArray();

	public bool AjouterFilm(Guid idFilm)
	{
		if (idFilm == Guid.Empty)
		{
			throw new ArgumentException("L'identifiant du film ne peut pas être vide.", nameof(idFilm));
		}

		return _realiseFilms.Add(idFilm);
	}

	public bool RetirerFilm(Guid idFilm)
	{
		return _realiseFilms.Remove(idFilm);
	}
}