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

public class RealisateursFavorisViewModel : Screen, IRealisateursFavorisViewModel
{
    private readonly IGestionnaireExceptions _gestionnaireExceptions;
    private readonly IRealisateurQueryService _realisateurQueryService;
    private readonly HashSet<Guid> _realisateursAAjouter = [];
    private readonly HashSet<Guid> _realisateursASupprimer = [];
    private readonly IRealisateursFavorisQueryService _realisateursFavorisQueryService;
    private readonly IRealisateursFavorisUpdateService _realisateursFavorisUpdateService;
    private readonly IWindowManager _windowManager;
    private bool _canSauvegarder;
    private byte _nbRealisateursSelectionnes;

    public RealisateursFavorisViewModel(IRealisateursFavorisQueryService realisateursFavorisQueryService,
        IRealisateursFavorisUpdateService realisateursFavorisUpdateService, IWindowManager windowManager,
        IGestionnaireExceptions gestionnaireExceptions, IRealisateurQueryService realisateurQueryService)
    {
        _realisateursFavorisUpdateService = realisateursFavorisUpdateService;
        _realisateursFavorisQueryService = realisateursFavorisQueryService;
        _realisateurQueryService = realisateurQueryService;
        _windowManager = windowManager;
        _gestionnaireExceptions = gestionnaireExceptions;

        _ = ChargerRealisateursEtRealisateursFavoris();
    }

    public byte NbMaxRealisateursFavoris => Utilisateur.MaxRealisateursFavoris;

    public byte NbRealisateursSelectionnes
    {
        get => _nbRealisateursSelectionnes;
        private set => SetAndNotify(ref _nbRealisateursSelectionnes, value);
    }

    public bool CanSauvegarder
    {
        get => _canSauvegarder;
        private set => SetAndNotify(ref _canSauvegarder, value);
    }

    public BindableCollection<SelectedItemWrapper<RealisateurDto>> Realisateurs { get; } = [];

    public async Task Sauvegarder()
    {
        if (!CanSauvegarder)
        {
            return;
        }

        await RetirerRealisateurs();
        await AjouterRealisateurs();
        CanSauvegarder = false;
    }

    public void OnSelectionChanged(SelectionChangedEventArgs evt)
    {
        ListBox listBox = (ListBox)evt.Source;
        SelectedItemWrapper<RealisateurDto>[] currentlySelectedItems =
            listBox.SelectedItems.Cast<SelectedItemWrapper<RealisateurDto>>().ToArray();

        _realisateursAAjouter.Clear();
        _realisateursASupprimer.Clear();

        foreach (SelectedItemWrapper<RealisateurDto> realisateurWrapper in Realisateurs)
        {
            switch (realisateurWrapper.IsSelected)
            {
                case true when currentlySelectedItems.All(a => a != realisateurWrapper):
                    _realisateursASupprimer.Add(realisateurWrapper.Item.Id);
                    break;
                case false when currentlySelectedItems.Any(a => a == realisateurWrapper):
                    _realisateursAAjouter.Add(realisateurWrapper.Item.Id);
                    break;
            }
        }

        NbRealisateursSelectionnes = (byte)listBox.SelectedItems.Count;
        CanSauvegarder = _realisateursAAjouter.Count > 0 || _realisateursASupprimer.Count > 0;
    }

    private async Task AjouterRealisateurs()
    {
        foreach (Guid guidRealisateur in _realisateursAAjouter.ToArray())
        {
            await _gestionnaireExceptions.GererExceptionAsync(async () =>
            {
                await _realisateursFavorisUpdateService.AjouterRealisateurFavori(guidRealisateur);
                Realisateurs.Single(realisateur => realisateur.Item.Id == guidRealisateur).IsSelected = true;
            });
            _realisateursAAjouter.Remove(guidRealisateur);
        }
    }

    private async Task RetirerRealisateurs()
    {
        foreach (Guid guidRealisateur in _realisateursASupprimer.ToArray())
        {
            await _gestionnaireExceptions.GererExceptionAsync(async () =>
            {
                await _realisateursFavorisUpdateService.RetirerRealisateurFavori(guidRealisateur);
                Realisateurs.Single(realisateur => realisateur.Item.Id == guidRealisateur).IsSelected = false;
            });
            _realisateursASupprimer.Remove(guidRealisateur);
        }
    }

    private async Task ChargerRealisateursEtRealisateursFavoris()
    {
        await ChargerTousLesRealisateurs();
        await ChargerRealisateursFavoris();
    }

    private async Task ChargerTousLesRealisateurs()
    {
        Realisateurs.Clear();
        IEnumerable<RealisateurDto> lstRealisateurs = await _realisateurQueryService.ObtenirTous();
        Realisateurs.AddRange(lstRealisateurs.Select(dto => new SelectedItemWrapper<RealisateurDto>(dto, false)));
    }

    private async Task ChargerRealisateursFavoris()
    {
        RealisateurDto[] realisateursFavoris =
            (await _realisateursFavorisQueryService.ObtenirRealisateursFavoris()).ToArray();
        NbRealisateursSelectionnes = 0;

        foreach (SelectedItemWrapper<RealisateurDto> realisateur in Realisateurs)
        {
            if (realisateursFavoris.All(af => af.Id != realisateur.Item.Id))
            {
                continue;
            }

            realisateur.IsSelected = true;
            ++NbRealisateursSelectionnes;
        }
    }
}