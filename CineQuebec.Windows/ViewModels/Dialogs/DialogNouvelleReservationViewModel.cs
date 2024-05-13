using System.Windows;

using CineQuebec.Application.Interfaces.Services.Projections;
using CineQuebec.Application.Records.Projections;
using CineQuebec.Windows.Interfaces;
using CineQuebec.Windows.Interfaces.ViewModels.Dialogs;

using Stylet;

namespace CineQuebec.Windows.ViewModels.Dialogs;

public class DialogNouvelleReservationViewModel(
    IBilletCreationService billetCreationService,
    ISalleQueryService salleQueryService,
    IWindowManager windowManager,
    IGestionnaireExceptions gestionnaireExceptions)
    : Screen, IDialogNouvelleReservationViewModel
{
    private ushort _nbBillets;
    private BindableCollection<ushort> _nbBilletsPossibles = [];
    private ProjectionDto _projection = default!;

    public bool AchatReussi { get; private set; }

    public ushort NbBillets
    {
        get => _nbBillets;
        set => SetAndNotify(ref _nbBillets, value);
    }


    public BindableCollection<ushort> NbBilletsPossibles
    {
        get => _nbBilletsPossibles;
        private set => SetAndNotify(ref _nbBilletsPossibles, value);
    }

    public ProjectionDto Projection
    {
        get => _projection;
        private set => SetAndNotify(ref _projection, value);
    }

    public void Annuler()
    {
        RequestClose(false);
    }

    public async Task Valider()
    {
        await gestionnaireExceptions.GererExceptionAsync(async () =>
        {
            await billetCreationService.ReserverProjection(Projection.Id, NbBillets);
            AchatReussi = true;
            RequestClose(true);
        });
    }

    public void SetData(object data)
    {
        if (data is not ProjectionDto projection)
        {
            return;
        }

        Projection = projection;
    }

    protected override void OnViewLoaded()
    {
        base.OnViewLoaded();
        _ = ObtenirNbBilletsDisponibles();
    }

    private async Task ObtenirNbBilletsDisponibles()
    {
        ushort nbPlacesRestantes = await salleQueryService.ObtenirNbPlacesRestantes(Projection.Id);

        if (nbPlacesRestantes == 0)
        {
            RequestClose(false);
            windowManager.ShowMessageBox("Il n'y a plus de places disponibles pour cette projection.",
                "Impossible de réserver", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        NbBilletsPossibles =
            new BindableCollection<ushort>(Enumerable.Range(1, Math.Min(nbPlacesRestantes, (ushort)30))
                .Select(i => (ushort)i));
    }
}