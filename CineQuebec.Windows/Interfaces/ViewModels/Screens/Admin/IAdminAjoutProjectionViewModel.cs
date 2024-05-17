using CineQuebec.Application.Records.Films;
using CineQuebec.Application.Records.Projections;
using CineQuebec.Windows.Interfaces.ViewModels.Components;

using Stylet;

namespace CineQuebec.Windows.Interfaces.ViewModels.Screens.Admin;

public interface IAdminAjoutProjectionViewModel : IScreenWithData
{
    BindableCollection<SalleDto> LstSalles { get; set; }
    SalleDto? SalleSelectionnee { get; set; }
    DateTime DateSelectionnee { get; set; }
    bool FormulairEstActive { get; set; }
    IHeaderViewModel HeaderViewModel { get; }
    FilmDto? Film { get; }
    bool EstAvantPremiere { get; set; }
    void AjouterSalle();
    void EnregistrerProjection();
    void Annuler();
    Task RefreshDetails();
}