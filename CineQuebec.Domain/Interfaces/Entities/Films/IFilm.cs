using System.Collections.Immutable;
using CineQuebec.Domain.Entities.Films;

namespace CineQuebec.Domain.Interfaces.Entities.Films;

public interface IFilm: IEntite
{
	string Titre { get; }
	string Description { get; }
	CategorieFilm Categorie { get; }
	DateTime DateSortieInternationale { get; }
	ImmutableArray<IActeur> Acteurs { get; }
	ImmutableArray<IRealisateur> Realisateurs { get; }
	ushort Duree { get; }
	void SetTitre(string titre);
	void SetDescription(string description);
	void SetCategorie(CategorieFilm categorie);
	void SetDateSortieInternationale(DateTime dateSortieInternationale);
	void AddActeurs(IEnumerable<IActeur> acteurs);
	void AddRealisateurs(IEnumerable<IRealisateur> realisateurs);
	void SetDuree(ushort duree);
}