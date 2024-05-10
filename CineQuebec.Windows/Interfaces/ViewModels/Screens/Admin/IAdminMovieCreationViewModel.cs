using System.Windows.Controls;

using CineQuebec.Application.Records.Films;
using CineQuebec.Windows.Interfaces.ViewModels.Components;
using CineQuebec.Windows.Records;

using Stylet;

namespace CineQuebec.Windows.Interfaces.ViewModels.Screens.Admin;

public interface IAdminMovieCreationViewModel : IScreenWithData
{
    IHeaderViewModel HeaderViewModel { get; }
    string TitreFilm { get; set; }
    string DescriptionFilm { get; set; }
    string DureeFilm { get; set; }
    BindableCollection<CategorieFilmDto> LstCategories { get; }
    BindableCollection<SelectedItemWrapper<ActeurDto>> LstActeurs { get; }
    BindableCollection<SelectedItemWrapper<RealisateurDto>> LstRealisateurs { get; }
    CategorieFilmDto? CategorieSelectionnee { get; set; }
    bool CanCreateFilm { get; }
    bool FormulairEstActive { get; }
    DateTime DateSelectionnee { get; set; }
    string TexteBoutonPrincipal { get; }
    void AjouterActeur();
    void AjouterRealisateur();
    void AjouterCategorie();
    void CreerFilm();
}