using System.Collections.Immutable;

namespace CineQuebec.Domain.Interfaces.Entities.Films;

public interface IRealisateur: IPersonne
{
	ImmutableArray<IFilm> RealiseFilms { get; }
	void AjouterFilm(IFilm film);
	void AjouterFilms(IEnumerable<IFilm> films);
	void RetirerFilm(IFilm film);
}