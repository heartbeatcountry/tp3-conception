using System.Windows.Controls;

using CineQuebec.Application.Records.Films;
using CineQuebec.Application.Records.Identity;
using CineQuebec.Application.Records.Projections;
using CineQuebec.Windows.Interfaces.ViewModels.Components;
using CineQuebec.Windows.Records;

using Stylet;

namespace CineQuebec.Windows.Interfaces.ViewModels.Screens.Admin;

public interface IAdminOffrirBilletsViewModel : IScreen
{
    IHeaderViewModel HeaderViewModel { get; }
    BindableCollection<FilmDto> LstFilms { get; }
    BindableCollection<ProjectionDto> LstProjections { get; }
    BindableCollection<SelectedItemWrapper<UtilisateurDto>> LstUtilisateurs { get; }
    FilmDto? FilmSelectionne { get; set; }
    ProjectionDto? ProjectionSelectionnee { get; set; }
    bool CanOffrirBillets { get; }
    void OnUtilisateurSelectionChange(SelectionChangedEventArgs evt);
    Task RafraichirFilms();
    Task OffrirBillets();
}