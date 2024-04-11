using System.Windows.Input;

using CineQuebec.Application.Interfaces.Services;
using CineQuebec.Application.Records.Films;
using CineQuebec.Application.Records.Projections;
using CineQuebec.Windows.Views.Components;

using Stylet;

namespace CineQuebec.Windows.Views;

public class AdminAjoutProjectionViewModel : Screen, IScreenWithData
{
    private readonly IFilmQueryService _filmQueryService;
    private readonly INavigationController _navigationController;
    private readonly ISalleQueryService _salleQueryService;
    private DateTime _dateSelectionnee = DateTime.Now;
    private Guid _filmId;
    private bool _formulairEstActive;
    private BindableCollection<SalleDto> _lstSalles = [];
    private SalleDto? _salleSelectionnee;

    public AdminAjoutProjectionViewModel(INavigationController navigationController, HeaderViewModel headerViewModel,
        IFilmQueryService filmQueryService)
    {
        _navigationController = navigationController;
        _filmQueryService = filmQueryService;
        headerViewModel.PreviousView = typeof(AdminHomeViewModel);
        HeaderViewModel = headerViewModel;
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

    public FilmDto Film { get; private set; }

    public void SetData(object data)
    {
        if (data is not Guid filmId)
        {
            return;
        }

        _filmId = filmId;
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
    }

    public void EnregistrerProjection()
    {
    }


    public void Annuler()
    {
        RequestClose(false);
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

        ActiverInterface();
    }
}