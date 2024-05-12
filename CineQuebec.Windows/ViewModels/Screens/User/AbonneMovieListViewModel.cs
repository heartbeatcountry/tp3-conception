using System.Windows;
using System.Windows.Input;

using CineQuebec.Application.Interfaces.Services.Films;
using CineQuebec.Application.Interfaces.Services.Identity;
using CineQuebec.Application.Interfaces.Services.Preferences;
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
    private readonly INoteFilmCreationService _noteFilmCreationService;
    private readonly IUtilisateurAuthenticationService _utilisateurAuthenticationService;
    private readonly Guid utilisateur;
    private bool _canRefreshFilms = true;
    private BindableCollection<FilmDto> _films;
    private FilmDto _filmSelectionne;
    //private NoteFilmDto _noteFilm;

    private StatutAffichage statut = StatutAffichage.AfficherAlAffiche;
    private bool AfficherDejaVusComboBox;

    public AbonneMovieListViewModel(INavigationController navigationController, IHeaderViewModel headerViewModel,
        IFilmQueryService filmQueryService, IGestionnaireExceptions gestionnaireExceptions,
        IUtilisateurAuthenticationService utilisateurAuthenticationService,
        INoteFilmCreationService noteFilmCreationService)
    {
        _navigationController = navigationController;
        _utilisateurAuthenticationService = utilisateurAuthenticationService;
        _filmQueryService = filmQueryService;
        _noteFilmCreationService = noteFilmCreationService;
        _gestionnaireExceptions = gestionnaireExceptions;
        HeaderViewModel = headerViewModel;
        HeaderViewModel.PreviousView = typeof(IHomeViewModel);

        utilisateur = _utilisateurAuthenticationService.ObtenirIdUtilisateurConnecte();

        _ = RefreshFilms();
    }



    public FilmDto FilmSelectionne
    {
        get => _filmSelectionne;
        set
        {
            if (_filmSelectionne != value)
            {
                _filmSelectionne = value;
                SetAndNotify(ref _filmSelectionne, value);
            }
        }
    }



    public bool CanRefreshFilms
    {
        get => _canRefreshFilms;
        set => SetAndNotify(ref _canRefreshFilms, value);
    }

    public BindableCollection<FilmDto> Films
    {
        get => _films;
        private set => SetAndNotify(ref _films, value);
    }

    public IHeaderViewModel HeaderViewModel { get; }

    public async Task RefreshFilms()
    {
        DesactiverInterface();
        IEnumerable<FilmDto> allFilms = [];

        try
        {
            switch (statut)
            {
                case StatutAffichage.AfficherTous:
                    AfficherDejaVusComboBox = false;
                    allFilms = await _filmQueryService.ObtenirTous();
                    break;
                case StatutAffichage.AfficherAlAffiche:
                    AfficherDejaVusComboBox = false;
                    allFilms = await _filmQueryService.ObtenirTousAlAffiche();
                    break;
                case StatutAffichage.AfficherDejaVus:

                    AfficherDejaVusComboBox = true;
                    allFilms = await _filmQueryService.ObtenirFilmsAssistesParUtilisateur();
                    break;
            }
        }
        catch (Exception exception)
        {
            _gestionnaireExceptions.GererException(exception);
            ActiverInterface();
            return;
        }

        Films = new BindableCollection<FilmDto>(allFilms);
        ActiverInterface();
    }


    public void ConsulterFilm(Guid id)
    {
        DesactiverInterface();
        _navigationController.NavigateTo<AbonneMovieDetailsViewModel>(id);
    }


    public void ChoisirAfficherTous()
    {
        if (statut == StatutAffichage.AfficherTous)
        {
            return;
        }

        statut = StatutAffichage.AfficherTous;

        _ = RefreshFilms();
    }

    public void ChoisirAfficherDejaVus()
    {
        if (statut == StatutAffichage.AfficherDejaVus)
        {
            return;
        }

        statut = StatutAffichage.AfficherDejaVus;
        _ = RefreshFilms();
    }

    public void ChoisirAfficherAlaffiche()
    {
        if (statut == StatutAffichage.AfficherAlAffiche)
        {
            return;
        }

        statut = StatutAffichage.AfficherAlAffiche;
        _ = RefreshFilms();
    }

    private void DesactiverInterface()
    {
        CanRefreshFilms = false;
        Mouse.OverrideCursor = Cursors.Wait;
    }

    private void ActiverInterface()
    {
        CanRefreshFilms = true;
        Mouse.OverrideCursor = null;
    }

    private enum StatutAffichage
    {
        AfficherTous,
        AfficherAlAffiche,
        AfficherDejaVus
    }
}