using System.Windows;
using System.Windows.Controls;

using CineQuebec.Application.Interfaces.Services;

using Stylet;

namespace CineQuebec.Windows.Views;

public class DialogInscriptionUtilisateurViewModel(
    IUtilisateurCreationService utilisateurCreationService,
    IUtilisateurAuthenticationService utilisateurAuthenticationService,
    GestionnaireExceptions gestionnaireExceptions) : Screen
{
    private string _confirmationMotDePasse = String.Empty;
    private string _courriel = String.Empty;
    private string _motDePasse = String.Empty;
    private string _nom = String.Empty;
    private string _prenom = String.Empty;

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
        _motDePasse = (sender as PasswordBox)?.Password ?? string.Empty;
    }

    public void OnConfirmationMdpChange(object sender, RoutedEventArgs e)
    {
        _confirmationMotDePasse = (sender as PasswordBox)?.Password ?? string.Empty;
    }

    private void ValiderConfirmationMotDePasse()
    {
        if (_motDePasse != _confirmationMotDePasse)
        {
            throw new ArgumentException("Les mots de passe ne correspondent pas.");
        }
    }

    private async Task TenterDeCreerLeCompte()
    {
        await utilisateurCreationService.CreerUtilisateurAsync(Nom, Prenom, Courriel, _motDePasse);
    }

    private async Task ConnecterLeCompte()
    {
        await utilisateurAuthenticationService.AuthentifierThreadAsync(Courriel, _motDePasse);
    }
}