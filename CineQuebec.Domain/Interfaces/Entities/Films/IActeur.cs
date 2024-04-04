using System.Collections.Immutable;

namespace CineQuebec.Domain.Interfaces.Entities.Films;

public interface IActeur : IPersonne
{
	ImmutableArray<Guid> JoueDansFilmsAvecId { get; }
	bool AjouterFilm(Guid idFilm);
	bool RetirerFilm(Guid idFilm);
}