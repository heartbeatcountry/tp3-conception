using System.Windows.Input;

using CineQuebec.Application.Interfaces.Services;
using CineQuebec.Application.Records.Films;
using CineQuebec.Windows.Views.Components;

using Stylet;

namespace CineQuebec.Windows.Views;

public class AdminMovieDetailsViewModel : Screen, IScreenWithData
{
    private readonly IFilmQueryService _filmQueryService;
    private readonly INavigationController _navigationController;
    private readonly IWindowManager _windowManager;
    private BindableCollection<ActeurDto> _acteurs = [];
    private bool _canRefreshDetails = true;
    private Guid _filmId;
    private BindableCollection<RealisateurDto> _realisateurs = [];

    public AdminMovieDetailsViewModel(INavigationController navigationController, HeaderViewModel headerViewModel,
        IFilmQueryService filmQueryService, IWindowManager windowManager)
    {
        _navigationController = navigationController;
        _filmQueryService = filmQueryService;
        _windowManager = windowManager;

        headerViewModel.PreviousView = typeof(AdminMovieListViewModel);
        HeaderViewModel = headerViewModel;
    }

    public FilmDto? Film { get; private set; }

    public bool CanRefreshDetails
    {
        get => _canRefreshDetails;
        set => SetAndNotify(ref _canRefreshDetails, value);
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

    public HeaderViewModel HeaderViewModel { get; }

    public void SetData(object data)
    {
        if (data is not Guid filmId)
        {
            return;
        }

        _filmId = filmId;
        _ = RefreshDetails();
    }

    public void AddNewFilm()
    {
        _navigationController.NavigateTo<MovieCreationViewModel>();
    }

    private void DesactiverInterface()
    {
        CanRefreshDetails = false;
        Mouse.OverrideCursor = Cursors.Wait;
    }

    private void ActiverInterface()
    {
        CanRefreshDetails = true;
        Mouse.OverrideCursor = null;
    }

    public async Task RefreshDetails()
    {
        DesactiverInterface();
        FilmDto? film = await _filmQueryService.ObtenirDetailsFilmParId(_filmId);

        if (film is null)
        {
            HeaderViewModel.GoBack();
            return;
        }

        Film = film;
        Acteurs = new BindableCollection<ActeurDto>(film.Acteurs);
        Realisateurs = new BindableCollection<RealisateurDto>(film.Realisateurs);
        ActiverInterface();
    }

    public void AjouterProjection(Guid id)
    {
        _navigationController.NavigateTo<AdminAjoutProjectionViewModel>();
    }

    public void ModifierFilm(Guid id)
    {
        _navigationController.NavigateTo<MovieModificationViewModel>();
    }
}