using CineQuebec.Domain.Interfaces.Entities.Abstract;

namespace CineQuebec.Domain.Interfaces.Entities.Films;

public interface INoteFilm : IEntite
{
    Guid IdUtilisateur { get; }
    Guid IdFilm { get; }
    byte Note { get; }
    void SetIdUtilisateur(Guid pIdUtilisateur);
    void SetIdFilm(Guid pIdFilm);
    void SetNote(byte pNoteObtenue);
}