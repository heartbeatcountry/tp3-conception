using CineQuebec.Application.Records.Films;
using CineQuebec.Application.Records.Projections;
using CineQuebec.Windows.Interfaces.ViewModels.Components;

using Stylet;

namespace CineQuebec.Windows.Interfaces.ViewModels.Screens.User;

public interface IAbonneMovieDetailsViewModel : IScreenWithData
{
    BindableCollection<ActeurDto> Acteurs { get; }
    FilmDto? Film { get; }
    IHeaderViewModel HeaderViewModel { get; }
    byte MaNote { get; }
    BindableCollection<ProjectionDto> Projections { get; }
    BindableCollection<RealisateurDto> Realisateurs { get; }
    void NoterFilm();
    void ReserverProjection(ProjectionDto projection);
}