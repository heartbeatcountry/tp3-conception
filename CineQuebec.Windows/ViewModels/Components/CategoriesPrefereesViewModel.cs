using System.Windows.Controls;

using CineQuebec.Application.Interfaces.Services.Films;
using CineQuebec.Application.Interfaces.Services.Preferences;
using CineQuebec.Application.Records.Films;
using CineQuebec.Domain.Entities.Utilisateurs;
using CineQuebec.Windows.Interfaces.ViewModels.Components;
using CineQuebec.Windows.Records;

using Stylet;

namespace CineQuebec.Windows.ViewModels.Components;

public class CategoriesPrefereesViewModel : Screen, ICategoriesPrefereesViewModel
{
    private readonly ICategorieFilmQueryService _categorieQueryService;
    private readonly HashSet<Guid> _categoriesAAjouter = [];
    private readonly HashSet<Guid> _categoriesASupprimer = [];
    private readonly ICategoriesPrefereesQueryService _categoriesPrefereesQueryService;
    private readonly ICategoriesPrefereesUpdateService _categoriesPrefereesUpdateService;
    private readonly IGestionnaireExceptions _gestionnaireExceptions;
    private readonly IWindowManager _windowManager;
    private bool _canSauvegarder;
    private byte _nbCategoriesSelectionnes;

    public CategoriesPrefereesViewModel(ICategoriesPrefereesQueryService categoriesPrefereesQueryService,
        ICategoriesPrefereesUpdateService categoriesPrefereesUpdateService, IWindowManager windowManager,
        IGestionnaireExceptions gestionnaireExceptions, ICategorieFilmQueryService categorieQueryService)
    {
        _categoriesPrefereesUpdateService = categoriesPrefereesUpdateService;
        _categoriesPrefereesQueryService = categoriesPrefereesQueryService;
        _categorieQueryService = categorieQueryService;
        _windowManager = windowManager;
        _gestionnaireExceptions = gestionnaireExceptions;

        _ = ChargerCategoriesEtCategoriesPreferees();
    }

    public byte NbMaxCategoriesPreferees => Utilisateur.MaxCategoriesPreferees;

    public byte NbCategoriesSelectionnes
    {
        get => _nbCategoriesSelectionnes;
        private set => SetAndNotify(ref _nbCategoriesSelectionnes, value);
    }

    public bool CanSauvegarder
    {
        get => _canSauvegarder;
        private set => SetAndNotify(ref _canSauvegarder, value);
    }

    public BindableCollection<SelectedItemWrapper<CategorieFilmDto>> Categories { get; } = [];

    public async Task Sauvegarder()
    {
        if (!CanSauvegarder)
        {
            return;
        }

        await RetirerCategories();
        await AjouterCategories();
        CanSauvegarder = false;
    }

    public void OnSelectionChanged(SelectionChangedEventArgs evt)
    {
        ListBox listBox = (ListBox)evt.Source;
        SelectedItemWrapper<CategorieFilmDto>[] currentlySelectedItems =
            listBox.SelectedItems.Cast<SelectedItemWrapper<CategorieFilmDto>>().ToArray();

        _categoriesAAjouter.Clear();
        _categoriesASupprimer.Clear();

        foreach (SelectedItemWrapper<CategorieFilmDto> categorieWrapper in Categories)
        {
            switch (categorieWrapper.IsSelected)
            {
                case true when currentlySelectedItems.All(a => a != categorieWrapper):
                    _categoriesASupprimer.Add(categorieWrapper.Item.Id);
                    break;
                case false when currentlySelectedItems.Any(a => a == categorieWrapper):
                    _categoriesAAjouter.Add(categorieWrapper.Item.Id);
                    break;
            }
        }

        NbCategoriesSelectionnes = (byte)listBox.SelectedItems.Count;
        CanSauvegarder = _categoriesAAjouter.Count > 0 || _categoriesASupprimer.Count > 0;
    }

    private async Task AjouterCategories()
    {
        foreach (Guid guidCategorie in _categoriesAAjouter.ToArray())
        {
            await _gestionnaireExceptions.GererExceptionAsync(async () =>
            {
                await _categoriesPrefereesUpdateService.AjouterCategoriePreferee(guidCategorie);
                Categories.Single(categorie => categorie.Item.Id == guidCategorie).IsSelected = true;
            });
            _categoriesAAjouter.Remove(guidCategorie);
        }
    }

    private async Task RetirerCategories()
    {
        foreach (Guid guidCategorie in _categoriesASupprimer.ToArray())
        {
            await _gestionnaireExceptions.GererExceptionAsync(async () =>
            {
                await _categoriesPrefereesUpdateService.RetirerCategoriePreferee(guidCategorie);
                Categories.Single(categorie => categorie.Item.Id == guidCategorie).IsSelected = false;
            });
            _categoriesASupprimer.Remove(guidCategorie);
        }
    }

    private async Task ChargerCategoriesEtCategoriesPreferees()
    {
        await ChargerTousLesCategories();
        await ChargerCategoriesPreferees();
    }

    private async Task ChargerTousLesCategories()
    {
        Categories.Clear();
        IEnumerable<CategorieFilmDto> lstCategories = await _categorieQueryService.ObtenirToutes();
        Categories.AddRange(lstCategories.Select(dto => new SelectedItemWrapper<CategorieFilmDto>(dto, false)));
    }

    private async Task ChargerCategoriesPreferees()
    {
        CategorieFilmDto[] categoriesPreferees =
            (await _categoriesPrefereesQueryService.ObtenirCategoriesPreferees()).ToArray();
        NbCategoriesSelectionnes = 0;

        foreach (SelectedItemWrapper<CategorieFilmDto> categorie in Categories)
        {
            if (categoriesPreferees.All(af => af.Id != categorie.Item.Id))
            {
                continue;
            }

            categorie.IsSelected = true;
            ++NbCategoriesSelectionnes;
        }
    }
}