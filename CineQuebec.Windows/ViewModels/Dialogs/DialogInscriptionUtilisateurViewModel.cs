using System.Windows;
using System.Windows.Controls;

using CineQuebec.Application.Interfaces.Services.Identity;
using CineQuebec.Windows.Interfaces.ViewModels.Dialogs;

using Stylet;

namespace CineQuebec.Windows.ViewModels.Dialogs;

public class DialogInscriptionUtilisateurViewModel(
    IUtilisateurCreationService utilisateurCreationService,
    IUtilisateurAuthenticationService utilisateurAuthenticationService,
    IGestionnaireExceptions gestionnaireExceptions) : Screen, IDialogInscriptionUtilisateurViewModel
{
    private string _courriel = string.Empty;
    private string _nom = string.Empty;
    private string _prenom = string.Empty;

    public bool InscriptionReussie { get; private set; }

    public string Nom
    {
        get => _nom;
        set => SetAndNotify(ref _nom, value);
    }

    public string Prenom
    {
        get => _prenom;
        set => SetAndNotify(ref _prenom, value);
    }

    public string Courriel
    {
        get => _courriel;
        set => SetAndNotify(ref _courriel, value);
    }

    public string MotDePasse { private get; set; } = string.Empty;
    public string ConfirmationMotDePasse { private get; set; } = string.Empty;

    public void Annuler()
    {
        RequestClose(false);
    }

    public async Task Valider()
    {
        try
        {
            ValiderConfirmationMotDePasse();
            await TenterDeCreerLeCompte();
            await ConnecterLeCompte();
        }
        catch (Exception ex)
        {
            gestionnaireExceptions.GererException(ex);
            return;
        }

        InscriptionReussie = true;
        RequestClose(true);
    }

    public void OnMdpChange(object sender, RoutedEventArgs e)
    {
        MotDePasse = (sender as PasswordBox)?.Password ?? string.Empty;
    }

    public void OnConfirmationMdpChange(object sender, RoutedEventArgs e)
    {
        ConfirmationMotDePasse = (sender as PasswordBox)?.Password ?? string.Empty;
    }

    private void ValiderConfirmationMotDePasse()
    {
        if (MotDePasse != ConfirmationMotDePasse)
        {
            throw new ArgumentException("Les mots de passe ne correspondent pas.");
        }
    }

    private async Task TenterDeCreerLeCompte()
    {
        await utilisateurCreationService.CreerUtilisateurAsync(Nom, Prenom, Courriel, MotDePasse);
    }

    private async Task ConnecterLeCompte()
    {
        await utilisateurAuthenticationService.AuthentifierThreadAsync(Courriel, MotDePasse);
    }
}