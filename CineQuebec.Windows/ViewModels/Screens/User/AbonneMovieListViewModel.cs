using System.Windows.Input;

using CineQuebec.Application.Interfaces.Services.Films;
using CineQuebec.Application.Records.Films;
using CineQuebec.Windows.Interfaces;
using CineQuebec.Windows.Interfaces.ViewModels.Components;
using CineQuebec.Windows.Interfaces.ViewModels.Screens;
using CineQuebec.Windows.Interfaces.ViewModels.Screens.User;

using Stylet;

namespace CineQuebec.Windows.ViewModels.Screens.User;

public class AbonneMovieListViewModel : Screen, IAbonneMovieListViewModel
{
    private readonly IFilmQueryService _filmQueryService;
    private readonly IGestionnaireExceptions _gestionnaireExceptions;
    private readonly INavigationController _navigationController;
    private BindableCollection<FilmDto> _films;

    private StatutAffichage _statut = StatutAffichage.AfficherAlAffiche;

    public AbonneMovieListViewModel(INavigationController navigationController, IHeaderViewModel headerViewModel,
        IFilmQueryService filmQueryService, IGestionnaireExceptions gestionnaireExceptions)
    {
        _navigationController = navigationController;
        _filmQueryService = filmQueryService;
        _gestionnaireExceptions = gestionnaireExceptions;
        _films = [];
        HeaderViewModel = headerViewModel;
        HeaderViewModel.PreviousView = typeof(IHomeViewModel);

        _ = RefreshFilms();
    }

    public BindableCollection<FilmDto> Films
    {
        get => _films;
        private set => SetAndNotify(ref _films, value);
    }

    public IHeaderViewModel HeaderViewModel { get; }


    public void ConsulterFilm(Guid id)
    {
        _navigationController.NavigateTo<AbonneMovieDetailsViewModel>(id);
    }


    public void ChoisirAfficherTous()
    {
        if (_statut == StatutAffichage.AfficherTous)
        {
            return;
        }

        _statut = StatutAffichage.AfficherTous;
        _ = RefreshFilms();
    }

    public void ChoisirAfficherDejaVus()
    {
        if (_statut == StatutAffichage.AfficherDejaVus)
        {
            return;
        }

        _statut = StatutAffichage.AfficherDejaVus;
        _ = RefreshFilms();
    }

    public void ChoisirAfficherAlaffiche()
    {
        if (_statut == StatutAffichage.AfficherAlAffiche)
        {
            return;
        }

        _statut = StatutAffichage.AfficherAlAffiche;
        _ = RefreshFilms();
    }

    public async Task RefreshFilms()
    {
        Mouse.OverrideCursor = Cursors.Wait;

        try
        {
            Films = new BindableCollection<FilmDto>(_statut switch
            {
                StatutAffichage.AfficherAlAffiche => await _filmQueryService.ObtenirTousAlAffiche(),
                StatutAffichage.AfficherDejaVus => await _filmQueryService.ObtenirFilmsAssistesParUtilisateur(),
                _ => await _filmQueryService.ObtenirTous()
            });
        }
        catch (Exception exception)
        {
            _gestionnaireExceptions.GererException(exception);
        }
        finally
        {
            Mouse.OverrideCursor = null;
        }
    }

    private enum StatutAffichage
    {
        AfficherTous,
        AfficherAlAffiche,
        AfficherDejaVus
    }
}