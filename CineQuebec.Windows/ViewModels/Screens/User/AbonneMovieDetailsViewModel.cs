using System.Windows.Input;

using CineQuebec.Application.Interfaces.Services.Films;
using CineQuebec.Application.Interfaces.Services.Preferences;
using CineQuebec.Application.Interfaces.Services.Projections;
using CineQuebec.Application.Records.Films;
using CineQuebec.Application.Records.Projections;
using CineQuebec.Windows.Interfaces;
using CineQuebec.Windows.Interfaces.ViewModels.Components;
using CineQuebec.Windows.Interfaces.ViewModels.Screens;

using Stylet;

namespace CineQuebec.Windows.ViewModels.Screens.User;

public class AbonneMovieDetailsViewModel : Screen, IScreenWithData, IAbonneMovieDetailsViewModel
{
    private readonly IFilmDeletionService _filmDeletionService;
    private readonly IFilmQueryService _filmQueryService;
    private readonly IGestionnaireExceptions _gestionnaireExceptions;
    private readonly INavigationController _navigationController;
    private readonly INoteFilmCreationService _noteFilmCreationService;
    private readonly IProjectionDeletionService _projectionDeletionService;
    private readonly IProjectionQueryService _projectionQueryService;
    private readonly IWindowManager _windowManager;
    private BindableCollection<ActeurDto> _acteurs = [];
    private bool _canRafraichirTout = true;
    private Guid _filmId;
    private NoteFilmDto _noteFilm;
    private BindableCollection<ProjectionDto> _projections = [];
    private BindableCollection<RealisateurDto> _realisateurs = [];

    public AbonneMovieDetailsViewModel(INavigationController navigationController, IHeaderViewModel headerViewModel,
        IFilmQueryService filmQueryService, IWindowManager windowManager, IFilmDeletionService filmDeletionService,
        IProjectionDeletionService projectionDeletionService, IProjectionQueryService projectionQueryService,
        INoteFilmCreationService noteFilmCreationService, IGestionnaireExceptions gestionnaireExceptions)
    {
        _navigationController = navigationController;
        _filmQueryService = filmQueryService;
        _filmDeletionService = filmDeletionService;
        _noteFilmCreationService = noteFilmCreationService;
        _windowManager = windowManager;
        _projectionDeletionService = projectionDeletionService;
        _projectionQueryService = projectionQueryService;
        _gestionnaireExceptions = gestionnaireExceptions;
        headerViewModel.PreviousView = typeof(AbonneMovieListViewModel);
        HeaderViewModel = headerViewModel;
    }


    public List<byte> NotesPossibles => Enumerable
        .Range(Domain.Entities.Films.NoteFilm.NoteMinimum, Domain.Entities.Films.NoteFilm.NoteMaximum)
        .Select(Convert.ToByte).ToList();

    public FilmDto? Film { get; private set; }

    public NoteFilmDto NoteFilm
    {
        get => _noteFilm;

        set
        {
            if (_noteFilm != value)
            {
                _noteFilm = value;
                SetAndNotify(ref _noteFilm, value);
            }
        }
    }


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


    public void RafraichirTout()
    {
        DesactiverInterface();
        _ = RafraichirDetails();
        _ = RafraichirProjections();
        ActiverInterface();
    }

    public void SetData(object data)
    {
        if (data is not Guid filmId)
        {
            return;
        }

        _filmId = filmId;
        RafraichirTout();
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

    private async Task ReserverProjectionAsync(Guid projectionId)
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