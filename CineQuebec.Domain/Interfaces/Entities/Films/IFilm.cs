using CineQuebec.Domain.Interfaces.Entities.Abstract;

namespace CineQuebec.Domain.Interfaces.Entities.Films;

public interface IFilm : IEntite
{
    string Titre { get; }
    string Description { get; }
    Guid IdCategorie { get; }
    DateTime DateSortieInternationale { get; }
    IEnumerable<Guid> ActeursParId { get; }
    IEnumerable<Guid> RealisateursParId { get; }
    ushort DureeEnMinutes { get; }
    ushort NoteMoyenne { get; }
    void SetTitre(string titre);
    void SetDescription(string description);
    void SetCategorie(Guid categorie);
    void SetDateSortieInternationale(DateTime dateSortieInternationale);
    void AddActeurs(IEnumerable<Guid> acteurs);
    void AddRealisateurs(IEnumerable<Guid> realisateurs);
    void SetDureeEnMinutes(ushort duree);
    void SetActeursParId(IEnumerable<Guid> acteurs);
    void SetRealisateursParId(IEnumerable<Guid> realisateurs);
    void SetNoteMoyenne(ushort noteMoyenne);
}