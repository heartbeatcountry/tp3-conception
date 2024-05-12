using System.Windows.Controls;

using CineQuebec.Application.Interfaces.Services.Fidelite;
using CineQuebec.Application.Interfaces.Services.Films;
using CineQuebec.Application.Interfaces.Services.Projections;
using CineQuebec.Application.Records.Films;
using CineQuebec.Application.Records.Identity;
using CineQuebec.Application.Records.Projections;
using CineQuebec.Windows.Interfaces;
using CineQuebec.Windows.Interfaces.ViewModels.Components;
using CineQuebec.Windows.Interfaces.ViewModels.Screens;
using CineQuebec.Windows.Interfaces.ViewModels.Screens.Admin;
using CineQuebec.Windows.Records;

using Stylet;

namespace CineQuebec.Windows.ViewModels.Screens.Admin;

public class AdminOffrirBilletsViewModel : Screen, IAdminOffrirBilletsViewModel
{
    private readonly IBilletCreationService _billetCreationService;
    private readonly IFilmQueryService _filmQueryService;
    private readonly IGestionnaireExceptions _gestionnaireExceptions;
    private readonly IProjectionQueryService _projectionQueryService;
    private readonly IUtilisateurFideliteQueryService _utilisateurFideliteQueryService;
    private readonly IWindowManager _windowManager;
    private bool _canOffrirBillets;
    private FilmDto? _filmSelectionne;
    private ProjectionDto? _projectionSelectionnee;

    public AdminOffrirBilletsViewModel(IHeaderViewModel headerViewModel,
        IFilmQueryService filmQueryService,
        IProjectionQueryService projectionQueryService,
        IUtilisateurFideliteQueryService utilisateurFideliteQueryService,
        IBilletCreationService billetCreationService,
        IWindowManager windowManager,
        IGestionnaireExceptions gestionnaireExceptions)
    {
        HeaderViewModel = headerViewModel;
        HeaderViewModel.PreviousView = typeof(IHomeViewModel);

        _filmQueryService = filmQueryService;
        _gestionnaireExceptions = gestionnaireExceptions;
        _projectionQueryService = projectionQueryService;
        _utilisateurFideliteQueryService = utilisateurFideliteQueryService;
        _billetCreationService = billetCreationService;
        _windowManager = windowManager;

        _ = RafraichirFilms();
    }

    public IHeaderViewModel HeaderViewModel { get; }
    public BindableCollection<FilmDto> LstFilms { get; } = [];
    public BindableCollection<ProjectionDto> LstProjections { get; } = [];
    public BindableCollection<SelectedItemWrapper<UtilisateurDto>> LstUtilisateurs { get; } = [];

    public FilmDto? FilmSelectionne
    {
        get => _filmSelectionne;
        set
        {
            SetAndNotify(ref _filmSelectionne, value);
            _ = RafraichirProjections();
        }
    }

    public ProjectionDto? ProjectionSelectionnee
    {
        get => _projectionSelectionnee;
        set
        {
            SetAndNotify(ref _projectionSelectionnee, value);
            _ = RafraichirUtilisateurs();
        }
    }

    public bool CanOffrirBillets
    {
        get => _canOffrirBillets;
        private set => SetAndNotify(ref _canOffrirBillets, value);
    }

    public async Task OffrirBillets()
    {
        if (ProjectionSelectionnee is null || !CanOffrirBillets)
        {
            return;
        }

        IEnumerable<Guid> utilisateursSelectionnes = LstUtilisateurs.Where(u => u.IsSelected).Select(u => u.Item.Id);

        await _gestionnaireExceptions.GererExceptionAsync(async () =>
        {
            foreach (Guid utilisateur in utilisateursSelectionnes)
            {
                await _billetCreationService.OffrirBilletGratuit(ProjectionSelectionnee.Id, utilisateur);
            }
        });

        _windowManager.ShowMessageBox("Billets octroyés avec succès.", "Succès");
        FilmSelectionne = null;
    }

    public void OnUtilisateurSelectionChange(SelectionChangedEventArgs evt)
    {
        CanOffrirBillets = LstUtilisateurs.Any(u => u.IsSelected);
    }

    public async Task RafraichirFilms()
    {
        LstFilms.Clear();

        await _gestionnaireExceptions.GererExceptionAsync(async () =>
            LstFilms.AddRange(await _filmQueryService.ObtenirTousAlAffiche()));

        FilmSelectionne ??= LstFilms.FirstOrDefault();
    }

    private async Task RafraichirProjections()
    {
        if (FilmSelectionne is null)
        {
            return;
        }

        LstProjections.Clear();

        await _gestionnaireExceptions.GererExceptionAsync(async () => LstProjections.AddRange(
            await _projectionQueryService.ObtenirProjectionsAVenirPourFilm(FilmSelectionne.Id)));

        ProjectionSelectionnee ??= LstProjections.FirstOrDefault();
    }

    private async Task RafraichirUtilisateurs()
    {
        if (ProjectionSelectionnee is null)
        {
            return;
        }

        LstUtilisateurs.Clear();

        await _gestionnaireExceptions.GererExceptionAsync(async () => LstUtilisateurs.AddRange(
            (await _utilisateurFideliteQueryService.ObtenirUtilisateursFideles(ProjectionSelectionnee.Id))
            .Select(u => new SelectedItemWrapper<UtilisateurDto>(u, false))));
    }
}