using System.Windows;
using System.Windows.Controls;

using CineQuebec.Application.Interfaces.Services;

using Stylet;

namespace CineQuebec.Windows.Views;

public class LoginViewModel(
    INavigationController navigationController,
    IUtilisateurAuthenticationService authenticationService,
    IDialogFactory dialogFactory,
    IWindowManager windowManager,
    GestionnaireExceptions gestionnaireExceptions)
    : Screen
{
    private string _motDePasse = String.Empty;
    private string _nomUsager = String.Empty;

    public string NomUsager
    {
        get => _nomUsager;
        set => SetAndNotify(ref _nomUsager, value.Trim());
    }

    public async Task SeConnecter()
    {
        try
        {
            await authenticationService.AuthentifierThreadAsync(NomUsager, _motDePasse);
        }
        catch (Exception exception)
        {
            gestionnaireExceptions.GererException(exception);
            return;
        }

        navigationController.NavigateTo<AdminHomeViewModel>();
    }

    public void OuvrirInscription()
    {
        DialogInscriptionUtilisateurViewModel dialog = dialogFactory.CreateDialogInscriptionUtilisateur();
        windowManager.ShowDialog(dialog);

        if (dialog.InscriptionReussie)
        {
            navigationController.NavigateTo<AdminHomeViewModel>();
        }
    }

    public void OnMdpChange(object sender, RoutedEventArgs e)
    {
        _motDePasse = (sender as PasswordBox)?.Password ?? string.Empty;
    }
}