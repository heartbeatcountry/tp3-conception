namespace CineQuebec.Domain.Interfaces.Entities.Projections;

public interface IProjection: IEntite
{
    Guid IdFilm { get; }
    Guid IdSalle { get; }
    DateTime DateHeure { get; }
    bool EstAvantPremiere { get; }
    void SetDateHeure(DateTime dateHeure);
    void SetEstAvantPremiere(bool estAvantPremiere);
    void SetFilm(Guid idFilm);
    void SetSalle(Guid idSalle);
}