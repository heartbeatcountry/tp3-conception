using CineQuebec.Application.Records.Films;
using CineQuebec.Application.Records.Projections;
using CineQuebec.Windows.Interfaces.ViewModels.Components;

using Stylet;

namespace CineQuebec.Windows.Interfaces.ViewModels.Screens.Admin;

public interface IAdminMovieDetailsViewModel : IScreenWithData
{
    FilmDto? Film { get; }
    bool CanRafraichirTout { get; set; }
    BindableCollection<ActeurDto> Acteurs { get; }
    BindableCollection<RealisateurDto> Realisateurs { get; }
    BindableCollection<ProjectionDto> Projections { get; }
    IHeaderViewModel HeaderViewModel { get; }
    void AddNewFilm();
    void RafraichirTout();
    void AjouterProjection();
    void ModifierFilm();
    void SupprimerFilm();
    void SupprimerProjection(ProjectionDto projection);
}