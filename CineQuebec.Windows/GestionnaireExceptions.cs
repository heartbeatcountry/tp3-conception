using System.Security;
using System.Windows;

using CineQuebec.Windows.Interfaces;
using CineQuebec.Windows.Interfaces.ViewModels.Screens;

using Stylet;

namespace CineQuebec.Windows;

public class GestionnaireExceptions(IWindowManager windowManager, INavigationController navigationController)
    : IGestionnaireExceptions
{
    public void GererException(Exception exception)
    {
        switch (exception)
        {
            case SecurityException securityException:
                GererException(securityException);
                return;
            case AggregateException aggregateException:
                GererException(aggregateException);
                return;
            default:
                AfficherMessage(exception.Message, "Problème dans le formulaire");
                return;
        }
    }

    public void GererException(Action action)
    {
        try
        {
            action();
        }
        catch (Exception exception)
        {
            GererException(exception);
        }
    }

    public async Task GererExceptionAsync(Func<Task> action)
    {
        try
        {
            await action();
        }
        catch (Exception exception)
        {
            GererException(exception);
        }
    }

    private void GererException(AggregateException aggregateException)
    {
        string message = aggregateException.InnerExceptions.Select(ObtenirMsgSansParametre)
            .Aggregate((a, b) => $"{a}\n{b}");

        AfficherMessage(message, "Problèmes dans le formulaire");
    }

    private void GererException(SecurityException securityException)
    {
        AfficherMessage(securityException.Message, "Problème d'authentification");
        navigationController.NavigateTo<ILoginViewModel>();
    }

    private void AfficherMessage(string message, string titre)
    {
        windowManager.ShowMessageBox(ObtenirMsgSansParametre(message), titre, MessageBoxButton.OK,
            MessageBoxImage.Warning);
    }

    private static string ObtenirMsgSansParametre(Exception exception)
    {
        return ObtenirMsgSansParametre(exception.InnerException?.Message ?? exception.Message);
    }

    private static string ObtenirMsgSansParametre(string message)
    {
        int indexOf = message.IndexOf(" (Parameter ", StringComparison.Ordinal);

        return indexOf == -1 ? message : message[..indexOf];
    }
}