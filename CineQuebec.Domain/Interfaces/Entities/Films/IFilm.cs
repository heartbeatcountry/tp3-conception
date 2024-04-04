using System.Collections.Immutable;
using CineQuebec.Domain.Entities.Films;

namespace CineQuebec.Domain.Interfaces.Entities.Films;

public interface IFilm : IEntite
{
	string Titre { get; }
	string Description { get; }
	Guid IdCategorie { get; }
	DateOnly DateSortieInternationale { get; }
	IEnumerable<Guid> ActeursParId { get; }
	IEnumerable<Guid> RealisateursParId { get; }
	ushort DureeEnMinutes { get; }
	void SetTitre(string titre);
	void SetDescription(string description);
	void SetCategorie(Guid categorie);
	void SetDateSortieInternationale(DateOnly dateSortieInternationale);
	void AddActeurs(IEnumerable<Guid> acteurs);
	void AddRealisateurs(IEnumerable<Guid> realisateurs);
	void SetActeurs(IEnumerable<Guid> acteurs);
	void SetRealisateurs(IEnumerable<Guid> realisateurs);
	void SetDureeEnMinutes(ushort duree);
}