using CineQuebec.Application.Records.Films;
using CineQuebec.Windows.Interfaces.ViewModels.Components;

using Stylet;

namespace CineQuebec.Windows.Interfaces.ViewModels.Screens.User;

public interface IAbonneMovieListViewModel : IScreen
{
    BindableCollection<FilmDto> Films { get; }
    IHeaderViewModel HeaderViewModel { get; }
    void ChoisirAfficherAlaffiche();
    void ChoisirAfficherDejaVus();
    void ChoisirAfficherTous();
    void ConsulterFilm(Guid id);
}