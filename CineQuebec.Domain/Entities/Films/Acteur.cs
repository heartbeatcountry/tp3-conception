using System.Collections.Immutable;
using CineQuebec.Domain.Entities.Abstract;
using CineQuebec.Domain.Interfaces.Entities.Films;

namespace CineQuebec.Domain.Entities.Films;

public class Acteur(string prenom, string nom) : Personne(prenom, nom), IActeur
{
	private readonly HashSet<Guid> _joueDansFilmsAvecId = [];

	private Acteur(Guid id, string prenom, string nom) : this(prenom, nom)
	{
		SetId(id);
	}

	public ImmutableArray<Guid> JoueDansFilmsAvecId => _joueDansFilmsAvecId.ToImmutableArray();

	public bool AjouterFilm(Guid idFilm)
	{
		if (idFilm == Guid.Empty)
		{
			throw new ArgumentException("L'identifiant du film ne peut pas être vide.", nameof(idFilm));
		}

		return _joueDansFilmsAvecId.Add(idFilm);
	}

	public bool RetirerFilm(Guid idFilm)
	{
		return _joueDansFilmsAvecId.Remove(idFilm);
	}
}