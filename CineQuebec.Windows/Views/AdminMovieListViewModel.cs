using System.Windows.Input;

using CineQuebec.Application.Interfaces.Services;
using CineQuebec.Application.Records.Films;
using CineQuebec.Windows.Views.Components;

using Stylet;

namespace CineQuebec.Windows.Views;

public class AdminMovieListViewModel : Screen
{
    private readonly IFilmQueryService _filmQueryService;
    private readonly INavigationController _navigationController;
    private bool _afficherUniquementAlaffiche;
    private bool _canRefreshFilms = true;
    private BindableCollection<FilmDto> _films;

    public AdminMovieListViewModel(INavigationController navigationController, HeaderViewModel headerViewModel,
        IFilmQueryService filmQueryService)
    {
        _navigationController = navigationController;
        _filmQueryService = filmQueryService;
        headerViewModel.PreviousView = typeof(AdminHomeViewModel);
        HeaderViewModel = headerViewModel;
        _ = RefreshFilms();
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

    public HeaderViewModel HeaderViewModel { get; }

    public void AddNewFilm()
    {
        _navigationController.NavigateTo<MovieCreationViewModel>();
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

    public async Task RefreshFilms()
    {
        DesactiverInterface();
        IEnumerable<FilmDto> allFilms = _afficherUniquementAlaffiche
            ? await _filmQueryService.ObtenirTousAlAffiche()
            : await _filmQueryService.ObtenirTous();
        Films = new BindableCollection<FilmDto>(allFilms);
        ActiverInterface();
    }

    public void AfficherTous()
    {
        if (!_afficherUniquementAlaffiche)
        {
            return;
        }

        _afficherUniquementAlaffiche = false;
        _ = RefreshFilms();
    }

    public void AfficherAlaffiche()
    {
        if (_afficherUniquementAlaffiche)
        {
            return;
        }

        _afficherUniquementAlaffiche = true;
        _ = RefreshFilms();
    }
}