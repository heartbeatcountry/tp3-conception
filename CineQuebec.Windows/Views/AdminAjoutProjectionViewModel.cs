using System.Windows;
using System.Windows.Input;

using CineQuebec.Application.Interfaces.Services;
using CineQuebec.Application.Records.Films;
using CineQuebec.Application.Records.Projections;
using CineQuebec.Application.Services;
using CineQuebec.Windows.Views.Components;

using Stylet;

namespace CineQuebec.Windows.Views;

public class AdminAjoutProjectionViewModel : Screen, IScreenWithData
{
    private readonly IFilmQueryService _filmQueryService;
    private readonly ISalleQueryService _salleQueryService;
    private readonly IProjectionCreationService _projectionCreationService;
    private readonly ISalleCreationService _salleCreationService;
    private readonly IWindowManager _windowManager;
    private readonly IDialogFactory _dialogFactory;
    private readonly GestionnaireExceptions _gestionnaireExceptions;

    private DateTime _dateSelectionnee = DateTime.Now;
    private Guid? _filmId;
    private bool _formulairEstActive = true;
    private BindableCollection<SalleDto> _lstSalles = [];
    private SalleDto? _salleSelectionnee;
    private FilmDto _film;
    private bool _estAvantPremiere = false;

    public AdminAjoutProjectionViewModel(HeaderViewModel headerViewModel, IFilmQueryService filmQueryService, IProjectionCreationService projectionCreationService, ISalleCreationService salleCreationService, ISalleQueryService salleQueryService, IWindowManager windowManager, IDialogFactory dialogFactory, GestionnaireExceptions gestionnaireExceptions)
    {
        _filmQueryService = filmQueryService;
        _projectionCreationService = projectionCreationService;
        _salleCreationService = salleCreationService;
        _salleQueryService = salleQueryService;
        _windowManager = windowManager;
        _dialogFactory = dialogFactory;
        _gestionnaireExceptions = gestionnaireExceptions;

        headerViewModel.PreviousView = typeof(AdminMovieDetailsViewModel);
        HeaderViewModel = headerViewModel;

        _ = ChargerSalles();
    }

    public BindableCollection<SalleDto> LstSalles
    {
        get => _lstSalles;
        set => SetAndNotify(ref _lstSalles, value);
    }

    public SalleDto? SalleSelectionnee
    {
        get => _salleSelectionnee;
        set => SetAndNotify(ref _salleSelectionnee, value);
    }


    public DateTime DateSelectionnee
    {
        get => _dateSelectionnee;
        set => SetAndNotify(ref _dateSelectionnee, value);
    }

    public bool FormulairEstActive
    {
        get => _formulairEstActive;
        set => SetAndNotify(ref _formulairEstActive, value);
    }

    public HeaderViewModel HeaderViewModel { get; }

    public FilmDto Film
    {
        get => _film;
        private set => SetAndNotify(ref _film, value);
    }

    public bool EstAvantPremiere
    {
        get => _estAvantPremiere;
        set => SetAndNotify(ref _estAvantPremiere, value);
    }

    public void SetData(object data)
    {
        if (data is not Guid filmId)
        {
            return;
        }

        _filmId = filmId;
        HeaderViewModel.PreviousViewData = _filmId;
        _ = RefreshDetails();
    }

    private async Task ChargerSalles()
    {
        DesactiverInterface();
        IEnumerable<SalleDto> salles = await _salleQueryService.ObtenirToutes();
        LstSalles = new BindableCollection<SalleDto>(salles);
        SalleSelectionnee = LstSalles.FirstOrDefault();
        ActiverInterface();
    }

    private void ActiverInterface()
    {
        FormulairEstActive = true;
        Mouse.OverrideCursor = null;
    }

    private void DesactiverInterface()
    {
        FormulairEstActive = false;
        Mouse.OverrideCursor = Cursors.Wait;
    }

    public void AjouterSalle()
    {
        var dialog = _dialogFactory.CreateDialogNouvelleSalle();
        dialog.DisplayName = "Ajouter un salle";
        _windowManager.ShowDialog(dialog);

        if (dialog.AValide)
        {
            AjouterSalle(dialog.Numero, dialog.Capacite);
        }
    }

    private async void AjouterSalle(byte numero, ushort capacite)
    {
        try
        {
            if (await _salleCreationService.CreerSalle(numero, capacite) is var nouvSalle)
            {
                _windowManager.ShowMessageBox("Salle ajoutée avec succès", "Succès", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                _ = ChargerSalles();
            }
        }
        catch (Exception e)
        {
            _gestionnaireExceptions.GererException(e);
        }
    }

    public void EnregistrerProjection()
    {
    }


    public void Annuler()
    {
        HeaderViewModel.GoBack();
    }

    public async Task RefreshDetails()
    {
        if (_filmId is null)
        {
            return;
        }

        DesactiverInterface();
        FilmDto? film = await _filmQueryService.ObtenirDetailsFilmParId((Guid)_filmId);

        if (film is null)
        {
            HeaderViewModel.GoBack();
            return;
        }

        Film = film;
        ActiverInterface();
    }
}