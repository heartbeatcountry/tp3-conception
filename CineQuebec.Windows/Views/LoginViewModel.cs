using CineQuebec.Application.Interfaces.Services;

using Stylet;

namespace CineQuebec.Windows.Views;

public class LoginViewModel(
    INavigationController navigationController,
    IAuthenticationService authenticationService,
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

    public string MotDePasse
    {
        get => _motDePasse;
        set => SetAndNotify(ref _motDePasse, value.Trim());
    }

    public async Task SeConnecter()
    {
        try
        {
            await authenticationService.AuthentifierThreadAsync(NomUsager, MotDePasse);
        }
        catch (Exception exception)
        {
            gestionnaireExceptions.GererException(exception);
            return;
        }

        navigationController.NavigateTo<AdminHomeViewModel>();
    }
}