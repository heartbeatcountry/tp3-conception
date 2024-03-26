using System.Collections.Immutable;

namespace CineQuebec.Domain.Interfaces.Entities.Films;

public interface IActeur: IPersonne
{
	ImmutableArray<IFilm> JoueDansFilms { get; }
	bool AjouterFilm(IFilm film);
	void AjouterFilms(IEnumerable<IFilm> films);
	bool RetirerFilm(IFilm film);
}