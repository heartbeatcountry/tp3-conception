using System.Windows.Input;

using CineQuebec.Application.Interfaces.Services.Films;
using CineQuebec.Application.Interfaces.Services.Preferences;
using CineQuebec.Application.Interfaces.Services.Projections;
using CineQuebec.Application.Records.Films;
using CineQuebec.Application.Records.Projections;
using CineQuebec.Windows.Interfaces;
using CineQuebec.Windows.Interfaces.ViewModels.Components;
using CineQuebec.Windows.Interfaces.ViewModels.Dialogs;
using CineQuebec.Windows.Interfaces.ViewModels.Screens.User;

using Stylet;

namespace CineQuebec.Windows.ViewModels.Screens.User;

public class AbonneMovieDetailsViewModel : Screen, IScreenWithData, IAbonneMovieDetailsViewModel
{
    private readonly IDialogFactory _dialogFactory;
    private readonly IFilmQueryService _filmQueryService;
    private readonly IGestionnaireExceptions _gestionnaireExceptions;
    private readonly INoteFilmCreationService _noteFilmCreationService;
    private readonly INoteFilmQueryService _noteFilmQueryService;
    private readonly IProjectionQueryService _projectionQueryService;
    private readonly IWindowManager _windowManager;
    private BindableCollection<ActeurDto> _acteurs = [];
    private FilmDto? _film;
    private Guid _filmId;
    private byte? _maNote;
    private BindableCollection<ProjectionDto> _projections = [];
    private BindableCollection<RealisateurDto> _realisateurs = [];

    public AbonneMovieDetailsViewModel(IHeaderViewModel headerViewModel, IFilmQueryService filmQueryService,
        IWindowManager windowManager, IProjectionQueryService projectionQueryService,
        INoteFilmCreationService noteFilmCreationService, IGestionnaireExceptions gestionnaireExceptions,
        IDialogFactory dialogFactory, INoteFilmQueryService noteFilmQueryService)
    {
        _filmQueryService = filmQueryService;
        _windowManager = windowManager;
        _projectionQueryService = projectionQueryService;
        _gestionnaireExceptions = gestionnaireExceptions;
        _noteFilmCreationService = noteFilmCreationService;
        _noteFilmQueryService = noteFilmQueryService;
        _dialogFactory = dialogFactory;
        headerViewModel.PreviousView = typeof(AbonneMovieListViewModel);
        HeaderViewModel = headerViewModel;
    }

    public FilmDto? Film
    {
        get => _film;
        private set => SetAndNotify(ref _film, value);
    }

    public byte MaNote
    {
        get => _maNote ?? 0;
        private set => SetAndNotify(ref _maNote, value);
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

    public void NoterFilm()
    {
        IDialogNoterFilmViewModel dialog = _dialogFactory.CreateDialogNoterFilm();
        dialog.DisplayName = "Noter ce film";
        _windowManager.ShowDialog(dialog);

        if (dialog.AValide)
        {
            _ = MettreAJourNoteFilmPourUtilisateur(dialog.NoteFilm);
        }
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


    public void RafraichirTout()
    {
        DesactiverInterface();
        _ = RafraichirDetails();
        _ = RafraichirProjections();
        ActiverInterface();
    }


    private async Task MettreAJourNoteFilmPourUtilisateur(byte nouvelleNote)
    {
        if (Film is null)
        {
            return;
        }

        await _gestionnaireExceptions.GererExceptionAsync(async () =>
        {
            await _noteFilmCreationService.NoterFilm(Film.Id, nouvelleNote);
        });
        _ = RafraichirDetails();
    }


    private static void DesactiverInterface()
    {
        Mouse.OverrideCursor = Cursors.Wait;
    }

    private static void ActiverInterface()
    {
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
        await RafraichirMaNote();
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

    private async Task RafraichirMaNote()
    {
        if (Film is null)
        {
            return;
        }

        await _gestionnaireExceptions.GererExceptionAsync(async () =>
            MaNote = await _noteFilmQueryService.ObtenirMaNotePourFilm(Film.Id) ?? 0);
    }
}