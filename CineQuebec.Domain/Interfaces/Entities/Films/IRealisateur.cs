using System.Collections.Immutable;

namespace CineQuebec.Domain.Interfaces.Entities.Films;

public interface IRealisateur: IPersonne
{
	ImmutableArray<IFilm> RealiseFilms { get; }
	bool AjouterFilm(IFilm film);
	void AjouterFilms(IEnumerable<IFilm> films);
	bool RetirerFilm(IFilm film);
}