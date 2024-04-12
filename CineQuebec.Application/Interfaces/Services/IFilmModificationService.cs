namespace CineQuebec.Application.Interfaces.Services;

public interface IFilmModificationService
{
    Task ModifierFilm(Guid idFilm, string titre, string description, Guid categorie, DateTime
        dateDeSortieInternationale, IEnumerable<Guid> acteurs, IEnumerable<Guid> realisateurs, ushort duree);
}