using CineQuebec.Application.Records.Films;
using CineQuebec.Windows.Interfaces.ViewModels.Components;

using Stylet;

namespace CineQuebec.Windows.Interfaces.ViewModels.Screens.Admin;

public interface IAdminMovieListViewModel : IScreen
{
    bool CanRefreshFilms { get; set; }
    BindableCollection<FilmDto> Films { get; }
    IHeaderViewModel HeaderViewModel { get; }
    void AddNewFilm();
    Task RefreshFilms();
    void AfficherTous();
    void AfficherAlaffiche();
    void ConsulterFilm(Guid id);
}