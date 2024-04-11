using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using CineQuebec.Application.Interfaces.Services;
using CineQuebec.Application.Records.Films;
using CineQuebec.Windows.Views.Components;

using Stylet;

namespace CineQuebec.Windows.Views;

public class MovieCreationViewModel : Screen
{
    private readonly IActeurCreationService _acteurCreationService;
    private readonly IActeurQueryService _acteurQueryService;
    private readonly ICategorieFilmCreationService _categorieFilmCreationService;
    private readonly ICategorieFilmQueryService _categorieFilmQueryService;
    private readonly IDialogFactory _dialogFactory;
    private readonly IFilmCreationService _filmCreationService;
    private readonly INavigationController _navigationController;
    private readonly IRealisateurCreationService _realisateurCreationService;
    private readonly IRealisateurQueryService _realisateurQueryService;
    private readonly IWindowManager _windowManager;
    private BindableCollection<ActeurDto> _acteursSelectionnes;
    private CategorieFilmDto? _categorieSelectionnee;
    private string _descriptionFilm;
    private string _dureeFilm;
    private bool _formulairEstActive;
    private BindableCollection<ActeurDto> _lstActeurs;
    private BindableCollection<CategorieFilmDto> _lstCategories;
    private BindableCollection<RealisateurDto> _lstRealisateurs;
    private BindableCollection<RealisateurDto> _realisateursSelectionnes;
    private string _titreFilm;

    public MovieCreationViewModel(INavigationController navigationController, IFilmCreationService filmCreationService,
        HeaderViewModel headerViewModel, IActeurQueryService acteurQueryService, IDialogFactory dialogFactory,
        IRealisateurQueryService realisateurQueryService, ICategorieFilmQueryService categorieFilmQueryService,
        IWindowManager windowManager, IActeurCreationService acteurCreationService,
        IRealisateurCreationService realisateurCreationService,
        ICategorieFilmCreationService categorieFilmCreationService)
    {
        _navigationController = navigationController;
        _filmCreationService = filmCreationService;
        _acteurQueryService = acteurQueryService;
        _realisateurQueryService = realisateurQueryService;
        _categorieFilmQueryService = categorieFilmQueryService;
        _acteurCreationService = acteurCreationService;
        _realisateurCreationService = realisateurCreationService;
        _categorieFilmCreationService = categorieFilmCreationService;
        _windowManager = windowManager;
        _dialogFactory = dialogFactory;

        HeaderViewModel = headerViewModel;
        headerViewModel.PreviousView = typeof(AdminMovieListViewModel);

        _ = ToutCharger();
    }

    public HeaderViewModel HeaderViewModel { get; }

    public string TitreFilm
    {
        get => _titreFilm;
        set => SetAndNotify(ref _titreFilm, value);
    }

    public string DescriptionFilm
    {
        get => _descriptionFilm;
        set => SetAndNotify(ref _descriptionFilm, value);
    }

    public string DureeFilm
    {
        get => _dureeFilm;
        set => SetAndNotify(ref _dureeFilm, value);
    }

    public BindableCollection<CategorieFilmDto> LstCategories
    {
        get => _lstCategories;
        set => SetAndNotify(ref _lstCategories, value);
    }

    public BindableCollection<ActeurDto> LstActeurs
    {
        get => _lstActeurs;
        set => SetAndNotify(ref _lstActeurs, value);
    }

    public BindableCollection<RealisateurDto> LstRealisateurs
    {
        get => _lstRealisateurs;
        set => SetAndNotify(ref _lstRealisateurs, value);
    }

    public BindableCollection<ActeurDto> ActeursSelectionnes
    {
        get => _acteursSelectionnes;
        set => SetAndNotify(ref _acteursSelectionnes, value);
    }

    public BindableCollection<RealisateurDto> RealisateursSelectionnes
    {
        get => _realisateursSelectionnes;
        set => SetAndNotify(ref _realisateursSelectionnes, value);
    }

    public CategorieFilmDto? CategorieSelectionnee
    {
        get => _categorieSelectionnee;
        set => SetAndNotify(ref _categorieSelectionnee, value);
    }

    public bool CanCreateFilm => !string.IsNullOrWhiteSpace(TitreFilm) && !string.IsNullOrWhiteSpace(DescriptionFilm) &&
                                 !string.IsNullOrWhiteSpace(DureeFilm) && FormulairEstActive;

    public bool FormulairEstActive
    {
        get => _formulairEstActive;
        set => SetAndNotify(ref _formulairEstActive, value);
    }

    private void DesactiverInterface()
    {
        FormulairEstActive = false;
        Mouse.OverrideCursor = Cursors.Wait;
    }

    private void ActiverInterface()
    {
        FormulairEstActive = true;
        Mouse.OverrideCursor = null;
    }

    private async Task ChargerCategories()
    {
        DesactiverInterface();
        IEnumerable<CategorieFilmDto> categories = await _categorieFilmQueryService.ObtenirToutes();
        LstCategories = new BindableCollection<CategorieFilmDto>(categories);
        CategorieSelectionnee = LstCategories.FirstOrDefault();
        ActiverInterface();
    }

    private async Task ChargerActeurs()
    {
        DesactiverInterface();
        IEnumerable<ActeurDto> acteurs = await _acteurQueryService.ObtenirTous();
        LstActeurs = new BindableCollection<ActeurDto>(acteurs);
        ActeursSelectionnes = [];
        ActiverInterface();
    }

    private async Task ChargerRealisateurs()
    {
        DesactiverInterface();
        IEnumerable<RealisateurDto> realisateurs = await _realisateurQueryService.ObtenirTous();
        LstRealisateurs = new BindableCollection<RealisateurDto>(realisateurs);
        RealisateursSelectionnes = [];
        ActiverInterface();
    }

    public async Task ToutCharger()
    {
        await ChargerCategories();
        await ChargerActeurs();
        await ChargerRealisateurs();
    }

    private void AfficherErreur(string msg)
    {
        _windowManager.ShowMessageBox(msg, "Problèmes dans le formulaire", MessageBoxButton.OK,
            MessageBoxImage.Warning);
    }

    public void OnActeursChange(object sender, SelectionChangedEventArgs evt)
    {
        ListBox listBox = (ListBox)sender;
        ActeursSelectionnes = new BindableCollection<ActeurDto>(listBox.SelectedItems.Cast<ActeurDto>());
    }

    public void OnRealisateursChange(object sender, SelectionChangedEventArgs evt)
    {
        ListBox listBox = (ListBox)sender;
        RealisateursSelectionnes = new BindableCollection<RealisateurDto>(listBox.SelectedItems.Cast<RealisateurDto>());
    }

    public void AjouterActeur()
    {
        DialogNomPrenomViewModel dialog = _dialogFactory.CreateDialogNomPrenom();
        dialog.DisplayName = "Ajouter un acteur";
        _windowManager.ShowDialog(dialog);

        if (dialog.AValide)
        {
            AjouterActeur(dialog.Prenom, dialog.Nom);
        }
    }

    private async void AjouterActeur(string prenom, string nom)
    {
        try
        {
            if (await _acteurCreationService.CreerActeur(prenom, nom) is var nouvActeur)
            {
                _windowManager.ShowMessageBox("Acteur ajouté avec succès", "Succès", MessageBoxButton.OK,
                                       MessageBoxImage.Information);
                _ = ChargerActeurs();
            }
        }
        catch (AggregateException e)
        {
            var message = e.InnerExceptions.Select(e => e.Message).Aggregate((a, b) => $"{a}\n{b}");
            AfficherErreur(message);
            return;
        }
    }

    public void AjouterRealisateur()
    {
        DialogNomPrenomViewModel dialog = _dialogFactory.CreateDialogNomPrenom();
        dialog.DisplayName = "Ajouter un réalisateur";
        _windowManager.ShowDialog(dialog);

        if (dialog.AValide)
        {
            AjouterRealisateur(dialog.Prenom, dialog.Nom);
        }
    }

    private async void AjouterRealisateur(string prenom, string nom)
    {
        try
        {
            if (await _realisateurCreationService.CreerRealisateur(prenom, nom) is var nouvRealisateur)
            {
                _windowManager.ShowMessageBox("Réalisateur ajouté avec succès", "Succès", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                _ = ChargerRealisateurs();
            }
        }
        catch (AggregateException e)
        {
            var message = e.InnerExceptions.Select(e => e.Message).Aggregate((a, b) => $"{a}\n{b}");
            AfficherErreur(message);
            return;
        }
    }

    public void AjouterCategorie()
    {
        DialogNomAffichageViewModel dialog = _dialogFactory.CreateDialogNomAffichage();
        dialog.DisplayName = "Ajouter une catégorie";
        _windowManager.ShowDialog(dialog);

        if (dialog.AValide)
        {
            AjouterCategorie(dialog.Nom);
        }
    }

    private async void AjouterCategorie(string nom)
    {
        try
        {
            if (await _categorieFilmCreationService.CreerCategorie(nom) is var nouvCategorie)
            {
                _windowManager.ShowMessageBox("Catégorie ajoutée avec succès", "Succès", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                _ = ChargerCategories();
            }
        }
        catch (AggregateException e)
        {
            var message = e.InnerExceptions.Select(e => e.Message).Aggregate((a, b) => $"{a}\n{b}");
            AfficherErreur(message);
            return;
        }
    }

    public void CreerFilm()
    {
        AjouterActeur();
    }
}