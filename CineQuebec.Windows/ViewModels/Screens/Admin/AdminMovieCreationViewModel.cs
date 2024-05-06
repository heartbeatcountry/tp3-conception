using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using CineQuebec.Application.Interfaces.Services.Films;
using CineQuebec.Application.Records.Films;
using CineQuebec.Windows.ViewModels.Components;
using CineQuebec.Windows.ViewModels.Dialogs;

using Stylet;

namespace CineQuebec.Windows.ViewModels.Screens.Admin;

public class AdminMovieCreationViewModel : Screen, IScreenWithData
{
    private readonly IActeurCreationService _acteurCreationService;
    private readonly IActeurQueryService _acteurQueryService;
    private readonly ICategorieFilmCreationService _categorieFilmCreationService;
    private readonly ICategorieFilmQueryService _categorieFilmQueryService;
    private readonly IDialogFactory _dialogFactory;
    private readonly IFilmCreationService _filmCreationService;
    private readonly IFilmQueryService _filmQueryService;
    private readonly IFilmUpdateService _filmUpdateService;
    private readonly IGestionnaireExceptions _gestionnaireExceptions;
    private readonly INavigationController _navigationController;
    private readonly IRealisateurCreationService _realisateurCreationService;
    private readonly IRealisateurQueryService _realisateurQueryService;
    private readonly IWindowManager _windowManager;
    private BindableCollection<ActeurDto> _acteursSelectionnes = [];
    private CategorieFilmDto? _categorieSelectionnee;
    private DateTime _dateSelectionnee = DateTime.Now;
    private string _descriptionFilm = string.Empty;
    private string _dureeFilm = string.Empty;
    private FilmDto? _film;
    private bool _formulairEstActive = true;
    private Guid? _idFilm;
    private BindableCollection<ActeurDto> _lstActeurs = [];
    private BindableCollection<CategorieFilmDto> _lstCategories = [];
    private BindableCollection<RealisateurDto> _lstRealisateurs = [];
    private BindableCollection<RealisateurDto> _realisateursSelectionnes = [];
    private string _texteBoutonPrincipal = "Créer le film";
    private string _titreFilm = string.Empty;

    public AdminMovieCreationViewModel(INavigationController navigationController,
        IFilmCreationService filmCreationService,
        HeaderViewModel headerViewModel, IActeurQueryService acteurQueryService, IDialogFactory dialogFactory,
        IRealisateurQueryService realisateurQueryService, ICategorieFilmQueryService categorieFilmQueryService,
        IWindowManager windowManager, IActeurCreationService acteurCreationService,
        IFilmUpdateService filmUpdateService,
        IRealisateurCreationService realisateurCreationService, IFilmQueryService filmQueryService,
        ICategorieFilmCreationService categorieFilmCreationService, IGestionnaireExceptions gestionnaireExceptions)
    {
        _navigationController = navigationController;
        _filmCreationService = filmCreationService;
        _acteurQueryService = acteurQueryService;
        _realisateurQueryService = realisateurQueryService;
        _categorieFilmQueryService = categorieFilmQueryService;
        _acteurCreationService = acteurCreationService;
        _realisateurCreationService = realisateurCreationService;
        _categorieFilmCreationService = categorieFilmCreationService;
        _filmUpdateService = filmUpdateService;
        _gestionnaireExceptions = gestionnaireExceptions;
        _filmQueryService = filmQueryService;
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
        set => SetAndNotify(ref _titreFilm, value.Trim());
    }

    public string DescriptionFilm
    {
        get => _descriptionFilm;
        set => SetAndNotify(ref _descriptionFilm, value.Trim());
    }

    public string DureeFilm
    {
        get => _dureeFilm;
        set => SetAndNotify(ref _dureeFilm, value.Trim());
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

    public DateTime DateSelectionnee
    {
        get => _dateSelectionnee;
        set => SetAndNotify(ref _dateSelectionnee, value);
    }

    public string TexteBoutonPrincipal
    {
        get => _texteBoutonPrincipal;
        set => SetAndNotify(ref _texteBoutonPrincipal, value);
    }

    public void SetData(object data)
    {
        if (data is not Guid idFilm)
        {
            return;
        }

        _idFilm = idFilm;
        _ = ChargerFilm();
    }

    public async Task ToutCharger()
    {
        await ChargerCategories();
        await ChargerActeurs();
        await ChargerRealisateurs();

        if (_film is not null)
        {
            ConfigurerEnModeEdition();
        }
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

    public async void CreerFilm()
    {
        List<Guid> guidsActeurs = ActeursSelectionnes.Select(a => a.Id).ToList();
        List<Guid> guidsRealisateurs = RealisateursSelectionnes.Select(r => r.Id).ToList();
        Guid? guidCategorie = CategorieSelectionnee?.Id;
        _ = ushort.TryParse(DureeFilm, out ushort duree);

        if (guidCategorie is null)
        {
            AfficherErreur("Veuillez sélectionner une catégorie");
            return;
        }

        try
        {
            if (_idFilm is { } id)
            {
                await _filmUpdateService.ModifierFilm(id, TitreFilm, DescriptionFilm, (Guid)guidCategorie!,
                    DateSelectionnee, guidsActeurs, guidsRealisateurs, duree);

                _windowManager.ShowMessageBox("Film modifié avec succès", "Succès", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                HeaderViewModel.GoBack();
            }

            else if (await _filmCreationService.CreerFilm(TitreFilm, DescriptionFilm, (Guid)guidCategorie!,
                         DateSelectionnee,
                         guidsActeurs, guidsRealisateurs, duree) is var nouvFilm)
            {
                _windowManager.ShowMessageBox("Film ajouté avec succès", "Succès", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                HeaderViewModel.GoBack();
            }
        }
        catch (Exception e)
        {
            _gestionnaireExceptions.GererException(e);
        }
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
        IEnumerable<CategorieFilmDto> categories;

        try
        {
            categories = await _categorieFilmQueryService.ObtenirToutes();
        }
        catch (Exception exception)
        {
            _gestionnaireExceptions.GererException(exception);
            ActiverInterface();
            return;
        }

        LstCategories = new BindableCollection<CategorieFilmDto>(categories);
        CategorieSelectionnee = LstCategories.FirstOrDefault();
        ActiverInterface();
    }

    private async Task ChargerActeurs()
    {
        DesactiverInterface();
        IEnumerable<ActeurDto> acteurs;

        try
        {
            acteurs = await _acteurQueryService.ObtenirTous();
        }
        catch (Exception exception)
        {
            _gestionnaireExceptions.GererException(exception);
            ActiverInterface();
            return;
        }

        LstActeurs = new BindableCollection<ActeurDto>(acteurs);
        ActeursSelectionnes = [];
        ActiverInterface();
    }

    private async Task ChargerRealisateurs()
    {
        DesactiverInterface();
        IEnumerable<RealisateurDto> realisateurs;

        try
        {
            realisateurs = await _realisateurQueryService.ObtenirTous();
        }
        catch (Exception exception)
        {
            _gestionnaireExceptions.GererException(exception);
            ActiverInterface();
            return;
        }

        LstRealisateurs = new BindableCollection<RealisateurDto>(realisateurs);
        RealisateursSelectionnes = [];
        ActiverInterface();
    }

    private void AfficherErreur(string msg)
    {
        _windowManager.ShowMessageBox(msg, "Problèmes dans le formulaire", MessageBoxButton.OK,
            MessageBoxImage.Warning);
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
        catch (Exception e)
        {
            _gestionnaireExceptions.GererException(e);
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
        catch (Exception e)
        {
            _gestionnaireExceptions.GererException(e);
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
        catch (Exception e)
        {
            _gestionnaireExceptions.GererException(e);
        }
    }

    private async Task ChargerFilm()
    {
        if (_idFilm is null)
        {
            return;
        }

        try
        {
            _film = await _filmQueryService.ObtenirDetailsFilmParId((Guid)_idFilm);
        }
        catch (Exception exception)
        {
            _gestionnaireExceptions.GererException(exception);
            return;
        }

        ConfigurerEnModeEdition();
    }

    private void ConfigurerEnModeEdition()
    {
        if (_film is null)
        {
            HeaderViewModel.GoBack();
            return;
        }

        TitreFilm = _film.Titre;
        DescriptionFilm = _film.Description;
        DureeFilm = _film.DureeEnMinutes.ToString(CultureInfo.InvariantCulture);
        DateSelectionnee = _film.DateSortieInternationale;
        CategorieSelectionnee = LstCategories.FirstOrDefault(c => c.Id == _film.Categorie?.Id);
        ActeursSelectionnes =
            new BindableCollection<ActeurDto>(LstActeurs.Where(a => _film.Acteurs.Any(a2 => a2.Id == a.Id)));
        RealisateursSelectionnes =
            new BindableCollection<RealisateurDto>(LstRealisateurs.Where(r =>
                _film.Realisateurs.Any(r2 => r2.Id == r.Id)));

        HeaderViewModel.DisplayName = "Modifier un film";
        HeaderViewModel.PreviousView = typeof(AdminMovieDetailsViewModel);
        HeaderViewModel.PreviousViewData = _film.Id;
        TexteBoutonPrincipal = "Modifier le film";
    }
}