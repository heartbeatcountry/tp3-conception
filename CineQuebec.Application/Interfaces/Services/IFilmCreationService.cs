namespace CineQuebec.Application.Interfaces.Services;

public interface IFilmCreationService
{
	Task<Guid> CreerFilm(string titre, string description, Guid categorie, DateTime
		dateDeSortieInternationale, IEnumerable<Guid> acteurs, IEnumerable<Guid> realisateurs, ushort duree);
}