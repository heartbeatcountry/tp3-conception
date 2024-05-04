using CineQuebec.Application.Records.Abstract;
using CineQuebec.Domain.Interfaces.Entities.Films;

namespace CineQuebec.Application.Records.Films;

public record class NoteFilmDto(
    Guid Id,
    FilmDto? IdFilm,
    ushort Note
) : EntityDto(Id);

internal static class NoteFilmExtensions
{
    internal static NoteFilmDto VersDto(this INoteFilm noteFilm, FilmDto? filmDto)
    {
        return new NoteFilmDto(noteFilm.Id, filmDto, noteFilm.Note);
    }
}