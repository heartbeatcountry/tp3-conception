using CineQuebec.Application.Interfaces.Services;
using CineQuebec.Application.Records.Films;
using CineQuebec.Application.Services;
using CineQuebec.Windows.Views.Components;

using Stylet;

namespace CineQuebec.Windows.Views;

public class AdminMovieListViewModel : Screen
{
    private readonly INavigationController _navigationController;
    private readonly IFilmQueryService _filmQueryService;
    private BindableCollection<FilmDto> _films;

    public BindableCollection<FilmDto> Films
    {
        get => _films;
        private set => SetAndNotify(ref _films, value);
    }

    public AdminMovieListViewModel(INavigationController navigationController, HeaderViewModel headerViewModel, IFilmQueryService filmQueryService)
    {
        _navigationController = navigationController;
        _filmQueryService = filmQueryService;
        headerViewModel.PreviousView = typeof(AdminHomeViewModel);
        HeaderViewModel = headerViewModel;
        _ = RefreshFilms();
    }

    public HeaderViewModel HeaderViewModel { get; }

    public async Task RefreshFilms()
    {
        var allFilms = await _filmQueryService.ObtenirTous();
        Films = new BindableCollection<FilmDto>(allFilms);
    }
}