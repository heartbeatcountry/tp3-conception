namespace CineQuebec.Domain.Interfaces.Entities.Films;

public interface ICategorieFilm : IEntite
{
	string NomAffichage { get; }
	void SetNomAffichage(string nomAffichage);
}