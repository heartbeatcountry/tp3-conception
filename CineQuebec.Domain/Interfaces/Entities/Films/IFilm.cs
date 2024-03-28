using System.Collections.Immutable;

namespace CineQuebec.Domain.Interfaces.Entities.Films;

public interface IFilm: IEntite
{
	string Titre { get; }
	string Description { get; }
	ICategorieFilm Categorie { get; }
	DateTime DateSortieInternationale { get; }
	ImmutableArray<IActeur> Acteurs { get; }
	ImmutableArray<IRealisateur> Realisateurs { get; }
	ushort Duree { get; }
	void SetTitre(string titre);
	void SetDescription(string description);
	void SetCategorie(ICategorieFilm categorie);
	void SetDateSortieInternationale(DateTime dateSortieInternationale);
	void AddActeurs(IEnumerable<IActeur> acteurs);
	void AddRealisateurs(IEnumerable<IRealisateur> realisateurs);
	void SetDuree(ushort duree);
}