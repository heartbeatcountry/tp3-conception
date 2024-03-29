using System.Collections.Immutable;

namespace CineQuebec.Domain.Interfaces.Entities.Films;

public interface IRealisateur: IPersonne
{
	ImmutableArray<IFilm> RealiseFilms { get; }
	bool AjouterFilm(IFilm film);
	bool RetirerFilm(IFilm film);
}