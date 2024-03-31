using System.Collections.Immutable;
using CineQuebec.Domain.Entities.Films;

namespace CineQuebec.Domain.Interfaces.Entities.Films;

public interface IFilm : IEntite
{
	string Titre { get; }
	string Description { get; }
	CategorieFilm Categorie { get; }
	DateOnly DateSortieInternationale { get; }
	IEnumerable<Acteur> Acteurs { get; }
	IEnumerable<Realisateur> Realisateurs { get; }
	ushort DureeEnMinutes { get; }
	void SetTitre(string titre);
	void SetDescription(string description);
	void SetCategorie(CategorieFilm categorie);
	void SetDateSortieInternationale(DateOnly dateSortieInternationale);
	void AddActeurs(IEnumerable<Acteur> acteurs);
	void AddRealisateurs(IEnumerable<Realisateur> realisateurs);
	void SetActeurs(IEnumerable<Acteur> acteurs);
	void SetRealisateurs(IEnumerable<Realisateur> realisateurs);
	void SetDureeEnMinutes(ushort duree);
}