using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using CineQuebec.Application.Interfaces.Services;
using CineQuebec.Windows.Interfaces.ViewModels.Dialogs;

using Stylet;

namespace CineQuebec.Windows.ViewModels.Screens;

public class LoginViewModel(
    INavigationController navigationController,
    IUtilisateurAuthenticationService authenticationService,
    IDialogFactory dialogFactory,
    IWindowManager windowManager,
    IGestionnaireExceptions gestionnaireExceptions)
    : Screen
{
    private bool _canOuvrirInscription = true;
    private bool _canSeConnecter = true;
    private bool _connexionBdEtablie;
    private string _motDePasse = string.Empty;
    private string _nomUsager = string.Empty;
    private Visibility _visibiliteTexteConnexion = Visibility.Hidden;

    public Visibility VisibiliteTexteConnexion
    {
        get => _visibiliteTexteConnexion;
        private set
        {
            if (_connexionBdEtablie)
            {
                return;
            }

            if (value == Visibility.Hidden)
            {
                _connexionBdEtablie = true;
            }

            SetAndNotify(ref _visibiliteTexteConnexion, value);
        }
    }

    public string NomUsager
    {
        get => _nomUsager;
        set => SetAndNotify(ref _nomUsager, value.Trim());
    }

    public bool CanSeConnecter
    {
        get => _canSeConnecter;
        private set => SetAndNotify(ref _canSeConnecter, value);
    }

    public bool CanOuvrirInscription
    {
        get => _canOuvrirInscription;
        private set => SetAndNotify(ref _canOuvrirInscription, value);
    }

    public async Task SeConnecter()
    {
        try
        {
            DesactiverInterface();
            await authenticationService.AuthentifierThreadAsync(NomUsager, _motDePasse);
        }
        catch (Exception exception)
        {
            ActiverInterface();
            gestionnaireExceptions.GererException(exception);
            return;
        }

        navigationController.NavigateTo<HomeViewModel>();
    }

    public void OuvrirInscription()
    {
        IDialogInscriptionUtilisateurViewModel dialog = dialogFactory.CreateDialogInscriptionUtilisateur();
        windowManager.ShowDialog(dialog);

        if (dialog.InscriptionReussie)
        {
            navigationController.NavigateTo<HomeViewModel>();
        }
    }

    public void OnMdpChange(object sender)
    {
        _motDePasse = (sender as PasswordBox)?.Password ?? string.Empty;
    }

    private void DesactiverInterface()
    {
        CanSeConnecter = false;
        CanOuvrirInscription = false;
        VisibiliteTexteConnexion = Visibility.Visible;
        Mouse.OverrideCursor = Cursors.Wait;
    }

    private void ActiverInterface()
    {
        CanSeConnecter = true;
        CanOuvrirInscription = true;
        VisibiliteTexteConnexion = Visibility.Hidden;
        Mouse.OverrideCursor = null;
    }
}