using CineQuebec.Application.Records.Films;
using CineQuebec.Application.Records.Projections;
using CineQuebec.Windows.Interfaces.ViewModels.Components;

using Stylet;

namespace CineQuebec.Windows.Interfaces.ViewModels.Screens;

public interface IAbonneMovieDetailsViewModel : IScreenWithData
{
    BindableCollection<ActeurDto> Acteurs { get; }
    bool CanRafraichirTout { get; }
    FilmDto? Film { get; }
    IHeaderViewModel HeaderViewModel { get; }
    NoteFilmDto NoteFilm { get; set; }
    List<byte> NotesPossibles { get; }
    BindableCollection<ProjectionDto> Projections { get; }
    BindableCollection<RealisateurDto> Realisateurs { get; }

    void RafraichirTout();
}