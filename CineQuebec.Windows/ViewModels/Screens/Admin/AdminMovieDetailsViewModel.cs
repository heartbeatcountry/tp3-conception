using System.Globalization;
using System.Windows;
using System.Windows.Input;

using CineQuebec.Application.Interfaces.Services.Films;
using CineQuebec.Application.Interfaces.Services.Projections;
using CineQuebec.Application.Records.Films;
using CineQuebec.Application.Records.Projections;
using CineQuebec.Windows.Interfaces;
using CineQuebec.Windows.Interfaces.ViewModels.Components;
using CineQuebec.Windows.Interfaces.ViewModels.Screens.Admin;

using Stylet;

namespace CineQuebec.Windows.ViewModels.Screens.Admin;

public class AdminMovieDetailsViewModel : Screen, IAdminMovieDetailsViewModel
{
    private readonly IFilmDeletionService _filmDeletionService;
    private readonly IFilmQueryService _filmQueryService;
    private readonly IGestionnaireExceptions _gestionnaireExceptions;
    private readonly INavigationController _navigationController;
    private readonly IProjectionDeletionService _projectionDeletionService;
    private readonly IProjectionQueryService _projectionQueryService;
    private readonly IWindowManager _windowManager;
    private BindableCollection<ActeurDto> _acteurs = [];
    private bool _canRafraichirTout = true;
    private Guid _filmId;
    private BindableCollection<ProjectionDto> _projections = [];
    private BindableCollection<RealisateurDto> _realisateurs = [];

    public AdminMovieDetailsViewModel(INavigationController navigationController, IHeaderViewModel headerViewModel,
        IFilmQueryService filmQueryService, IWindowManager windowManager, IFilmDeletionService filmDeletionService,
        IProjectionDeletionService projectionDeletionService, IProjectionQueryService projectionQueryService,
        IGestionnaireExceptions gestionnaireExceptions)
    {
        _navigationController = navigationController;
        _filmQueryService = filmQueryService;
        _filmDeletionService = filmDeletionService;
        _windowManager = windowManager;
        _projectionDeletionService = projectionDeletionService;
        _projectionQueryService = projectionQueryService;
        _gestionnaireExceptions = gestionnaireExceptions;

        headerViewModel.PreviousView = typeof(IAdminMovieListViewModel);
        HeaderViewModel = headerViewModel;
    }

    public FilmDto? Film { get; private set; }

    public bool CanRafraichirTout
    {
        get => _canRafraichirTout;
        set => SetAndNotify(ref _canRafraichirTout, value);
    }

    public BindableCollection<ActeurDto> Acteurs
    {
        get => _acteurs;
        private set => SetAndNotify(ref _acteurs, value);
    }

    public BindableCollection<RealisateurDto> Realisateurs
    {
        get => _realisateurs;
        private set => SetAndNotify(ref _realisateurs, value);
    }

    public BindableCollection<ProjectionDto> Projections
    {
        get => _projections;
        private set => SetAndNotify(ref _projections, value);
    }

    public IHeaderViewModel HeaderViewModel { get; }

    public void SetData(object data)
    {
        if (data is not Guid filmId)
        {
            return;
        }

        _filmId = filmId;
        RafraichirTout();
    }

    public void AddNewFilm()
    {
        _navigationController.NavigateTo<IAdminMovieCreationViewModel>();
    }

    public void RafraichirTout()
    {
        DesactiverInterface();
        _ = RafraichirDetails();
        _ = RafraichirProjections();
        ActiverInterface();
    }

    public void AjouterProjection()
    {
        if (Film is null)
        {
            return;
        }

        _navigationController.NavigateTo<IAdminAjoutProjectionViewModel>(Film.Id);
    }

    public void ModifierFilm()
    {
        if (Film is null)
        {
            return;
        }

        _navigationController.NavigateTo<IAdminMovieCreationViewModel>(Film.Id);
    }

    public async void SupprimerFilm()
    {
        if (Film is null)
        {
            return;
        }

        MessageBoxResult result = _windowManager.ShowMessageBox(
            $"Êtes-vous certain.e de vouloir supprimer le film {Film.Titre} ?",
            "Supprimer un film", MessageBoxButton.YesNo);

        if (result != MessageBoxResult.Yes)
        {
            return;
        }

        DesactiverInterface();

        try
        {
            await _filmDeletionService.SupprimerFilm(Film.Id);
        }
        catch (Exception exception)
        {
            _gestionnaireExceptions.GererException(exception);
            return;
        }

        _windowManager.ShowMessageBox("Le film a été supprimé avec succès.", "Suppression de film");
        HeaderViewModel.GoBack();
    }

    public void SupprimerProjection(ProjectionDto projection)
    {
        if (Film is null)
        {
            return;
        }

        string dateFormatee = projection.DateHeure.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        MessageBoxResult result = _windowManager.ShowMessageBox(
            $"Êtes-vous certain.e de vouloir supprimer la projection du {dateFormatee} pour le film {Film.Titre} ?",
            "Supprimer une projection", MessageBoxButton.YesNo);

        if (result != MessageBoxResult.Yes)
        {
            return;
        }

        _ = SupprimerProjectionAsync(projection.Id);
        _windowManager.ShowMessageBox("La projection a été supprimée avec succès.", "Suppression de projection");
    }

    private void DesactiverInterface()
    {
        CanRafraichirTout = false;
        Mouse.OverrideCursor = Cursors.Wait;
    }

    private void ActiverInterface()
    {
        CanRafraichirTout = true;
        Mouse.OverrideCursor = null;
    }

    private async Task RafraichirDetails()
    {
        FilmDto? film;

        try
        {
            film = await _filmQueryService.ObtenirDetailsFilmParId(_filmId);
        }
        catch (Exception exception)
        {
            _gestionnaireExceptions.GererException(exception);
            return;
        }

        if (film is null)
        {
            HeaderViewModel.GoBack();
            return;
        }

        Film = film;
        Acteurs = new BindableCollection<ActeurDto>(film.Acteurs);
        Realisateurs = new BindableCollection<RealisateurDto>(film.Realisateurs);
    }

    private async Task RafraichirProjections()
    {
        if (Film is null)
        {
            return;
        }

        IEnumerable<ProjectionDto> projections;

        try
        {
            projections = await _projectionQueryService.ObtenirProjectionsAVenirPourFilm(Film.Id);
        }
        catch (Exception exception)
        {
            _gestionnaireExceptions.GererException(exception);
            return;
        }

        Projections = new BindableCollection<ProjectionDto>(projections);
    }

    private async Task SupprimerProjectionAsync(Guid projectionId)
    {
        DesactiverInterface();
        bool success = false;

        try
        {
            success = await _projectionDeletionService.SupprimerProjection(projectionId);
        }
        catch (Exception exception)
        {
            _gestionnaireExceptions.GererException(exception);
        }

        if (success)
        {
            await RafraichirProjections();
        }

        ActiverInterface();
    }
}