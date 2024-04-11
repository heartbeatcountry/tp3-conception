using System.Globalization;
using System.Windows;
using System.Windows.Input;

using CineQuebec.Application.Interfaces.Services;
using CineQuebec.Application.Records.Films;
using CineQuebec.Application.Records.Projections;
using CineQuebec.Windows.Views.Components;

using Stylet;

namespace CineQuebec.Windows.Views;

public class AdminMovieDetailsViewModel : Screen, IScreenWithData
{
    private readonly IFilmQueryService _filmQueryService;
    private readonly INavigationController _navigationController;
    private readonly IProjectionDeletionService _projectionDeletionService;
    private readonly IProjectionQueryService _projectionQueryService;
    private readonly IWindowManager _windowManager;
    private BindableCollection<ActeurDto> _acteurs = [];
    private bool _canRafraichirTout = true;
    private Guid _filmId;
    private BindableCollection<RealisateurDto> _realisateurs = [];

    public AdminMovieDetailsViewModel(INavigationController navigationController, HeaderViewModel headerViewModel,
        IFilmQueryService filmQueryService, IWindowManager windowManager,
        IProjectionDeletionService projectionDeletionService, IProjectionQueryService projectionQueryService)
    {
        _navigationController = navigationController;
        _filmQueryService = filmQueryService;
        _windowManager = windowManager;
        _projectionDeletionService = projectionDeletionService;
        _projectionQueryService = projectionQueryService;

        headerViewModel.PreviousView = typeof(AdminMovieListViewModel);
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

    public BindableCollection<ProjectionDto> Projections { get; private set; } = [];

    public HeaderViewModel HeaderViewModel { get; }

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
        _navigationController.NavigateTo<MovieCreationViewModel>();
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
        FilmDto? film = await _filmQueryService.ObtenirDetailsFilmParId(_filmId);

        if (film is null)
        {
            HeaderViewModel.GoBack();
            return;
        }

        Film = film;
        Acteurs = new BindableCollection<ActeurDto>(film.Acteurs);
        Realisateurs = new BindableCollection<RealisateurDto>(film.Realisateurs);
    }

    public void RafraichirTout()
    {
        DesactiverInterface();
        _ = RafraichirDetails();
        _ = RafraichirProjections();
        ActiverInterface();
    }

    private async Task RafraichirProjections()
    {
        if (Film is null)
        {
            return;
        }

        IEnumerable<ProjectionDto>
            projections = await _projectionQueryService.ObtenirProjectionsAVenirPourFilm(Film.Id);
        Projections = new BindableCollection<ProjectionDto>(projections);
    }

    public void AjouterProjection()
    {
        if (Film is null)
        {
            return;
        }

        _navigationController.NavigateTo<AdminAjoutProjectionViewModel>(Film.Id);
    }

    public void ModifierFilm()
    {
        if (Film is null)
        {
            return;
        }

        _navigationController.NavigateTo<MovieModificationViewModel>(Film.Id);
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

    private async Task SupprimerProjectionAsync(Guid projectionId)
    {
        DesactiverInterface();
        bool success = await _projectionDeletionService.SupprimerProjection(projectionId);

        if (success)
        {
            await RafraichirProjections();
        }

        ActiverInterface();
    }
}