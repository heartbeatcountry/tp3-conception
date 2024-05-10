using System.Windows.Controls;

using CineQuebec.Application.Records.Films;
using CineQuebec.Windows.Interfaces.ViewModels.Components;

using Stylet;

namespace CineQuebec.Windows.Interfaces.ViewModels.Screens.Admin;

public interface IAdminMovieCreationViewModel : IScreenWithData
{
    IHeaderViewModel HeaderViewModel { get; }
    string TitreFilm { get; set; }
    string DescriptionFilm { get; set; }
    string DureeFilm { get; set; }
    BindableCollection<CategorieFilmDto> LstCategories { get; set; }
    BindableCollection<ActeurDto> LstActeurs { get; set; }
    BindableCollection<RealisateurDto> LstRealisateurs { get; set; }
    BindableCollection<ActeurDto> ActeursSelectionnes { get; set; }
    BindableCollection<RealisateurDto> RealisateursSelectionnes { get; set; }
    CategorieFilmDto? CategorieSelectionnee { get; set; }
    bool CanCreateFilm { get; }
    bool FormulairEstActive { get; set; }
    DateTime DateSelectionnee { get; set; }
    string TexteBoutonPrincipal { get; set; }
    Task ToutCharger();
    void OnActeursChange(object sender, SelectionChangedEventArgs evt);
    void OnRealisateursChange(object sender, SelectionChangedEventArgs evt);
    void AjouterActeur();
    void AjouterRealisateur();
    void AjouterCategorie();
    void CreerFilm();
}