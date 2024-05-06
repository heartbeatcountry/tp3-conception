namespace CineQuebec.Application.Interfaces.Services.Films;

public interface IFilmUpdateService
{
    Task ModifierFilm(Guid idFilm, string titre, string description, Guid categorie, DateTime
        dateDeSortieInternationale, IEnumerable<Guid> acteurs, IEnumerable<Guid> realisateurs, ushort duree);
}