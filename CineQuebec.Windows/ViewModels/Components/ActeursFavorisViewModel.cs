using System.Windows.Controls;

using CineQuebec.Application.Interfaces.Services.Films;
using CineQuebec.Application.Interfaces.Services.Preferences;
using CineQuebec.Application.Records.Films;
using CineQuebec.Domain.Entities.Utilisateurs;
using CineQuebec.Windows.Interfaces;
using CineQuebec.Windows.Interfaces.ViewModels.Components;
using CineQuebec.Windows.Records;

using Stylet;

namespace CineQuebec.Windows.ViewModels.Components;

public class ActeursFavorisViewModel : Screen, IActeursFavorisViewModel
{
    private readonly IActeurQueryService _acteurQueryService;
    private readonly HashSet<Guid> _acteursAAjouter = [];
    private readonly HashSet<Guid> _acteursASupprimer = [];
    private readonly IActeursFavorisQueryService _acteursFavorisQueryService;
    private readonly IActeursFavorisUpdateService _acteursFavorisUpdateService;
    private readonly IGestionnaireExceptions _gestionnaireExceptions;
    private bool _canSauvegarder;
    private byte _nbActeursSelectionnes;

    public ActeursFavorisViewModel(IActeursFavorisQueryService acteursFavorisQueryService,
        IActeursFavorisUpdateService acteursFavorisUpdateService, IGestionnaireExceptions gestionnaireExceptions,
        IActeurQueryService acteurQueryService)
    {
        _acteursFavorisUpdateService = acteursFavorisUpdateService;
        _acteursFavorisQueryService = acteursFavorisQueryService;
        _acteurQueryService = acteurQueryService;
        _gestionnaireExceptions = gestionnaireExceptions;

        _ = ChargerActeursEtActeursFavoris();
    }

    public byte NbMaxActeursFavoris => Utilisateur.MaxActeursFavoris;

    public byte NbActeursSelectionnes
    {
        get => _nbActeursSelectionnes;
        private set => SetAndNotify(ref _nbActeursSelectionnes, value);
    }

    public bool CanSauvegarder
    {
        get => _canSauvegarder;
        private set => SetAndNotify(ref _canSauvegarder, value);
    }

    public BindableCollection<SelectedItemWrapper<ActeurDto>> Acteurs { get; } = [];

    public async Task Sauvegarder()
    {
        if (!CanSauvegarder)
        {
            return;
        }

        await RetirerActeurs();
        await AjouterActeurs();
        CanSauvegarder = false;
    }

    public void OnSelectionChanged(SelectionChangedEventArgs evt)
    {
        ListBox listBox = (ListBox)evt.Source;
        SelectedItemWrapper<ActeurDto>[] currentlySelectedItems =
            listBox.SelectedItems.Cast<SelectedItemWrapper<ActeurDto>>().ToArray();

        _acteursAAjouter.Clear();
        _acteursASupprimer.Clear();

        foreach (SelectedItemWrapper<ActeurDto> acteurWrapper in Acteurs)
        {
            switch (acteurWrapper.IsSelected)
            {
                case true when currentlySelectedItems.All(a => a != acteurWrapper):
                    _acteursASupprimer.Add(acteurWrapper.Item.Id);
                    break;
                case false when currentlySelectedItems.Any(a => a == acteurWrapper):
                    _acteursAAjouter.Add(acteurWrapper.Item.Id);
                    break;
            }
        }

        NbActeursSelectionnes = (byte)listBox.SelectedItems.Count;
        CanSauvegarder = _acteursAAjouter.Count > 0 || _acteursASupprimer.Count > 0;
    }

    private async Task AjouterActeurs()
    {
        foreach (Guid guidActeur in _acteursAAjouter.ToArray())
        {
            await _gestionnaireExceptions.GererExceptionAsync(async () =>
            {
                await _acteursFavorisUpdateService.AjouterActeurFavori(guidActeur);
                Acteurs.Single(acteur => acteur.Item.Id == guidActeur).IsSelected = true;
            });
            _acteursAAjouter.Remove(guidActeur);
        }
    }

    private async Task RetirerActeurs()
    {
        foreach (Guid guidActeur in _acteursASupprimer.ToArray())
        {
            await _gestionnaireExceptions.GererExceptionAsync(async () =>
            {
                await _acteursFavorisUpdateService.RetirerActeurFavori(guidActeur);
                Acteurs.Single(acteur => acteur.Item.Id == guidActeur).IsSelected = false;
            });
            _acteursASupprimer.Remove(guidActeur);
        }
    }

    private async Task ChargerActeursEtActeursFavoris()
    {
        await ChargerTousLesActeurs();
        await ChargerActeursFavoris();
    }

    private async Task ChargerTousLesActeurs()
    {
        Acteurs.Clear();
        IEnumerable<ActeurDto> lstActeurs = await _acteurQueryService.ObtenirTous();
        Acteurs.AddRange(lstActeurs.Select(dto => new SelectedItemWrapper<ActeurDto>(dto, false)));
    }

    private async Task ChargerActeursFavoris()
    {
        ActeurDto[] acteursFavoris = (await _acteursFavorisQueryService.ObtenirActeursFavoris()).ToArray();
        NbActeursSelectionnes = 0;

        foreach (SelectedItemWrapper<ActeurDto> acteur in Acteurs)
        {
            if (acteursFavoris.All(af => af.Id != acteur.Item.Id))
            {
                continue;
            }

            acteur.IsSelected = true;
            ++NbActeursSelectionnes;
        }
    }
}