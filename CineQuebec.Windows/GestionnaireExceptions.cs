using System.Windows;

using Stylet;

namespace CineQuebec.Windows;

public class GestionnaireExceptions(IWindowManager windowManager)
{
    private static string ObtenirMsgSansParametre(Exception exception)
    {
        string message = exception.InnerException?.Message ?? exception.Message;
        int indexOf = message.IndexOf(" (Parameter ", StringComparison.Ordinal);

        return indexOf == -1 ? message : message[..indexOf];
    }

    public void GererException(AggregateException aggregateException)
    {
        string message = aggregateException.InnerExceptions.Select(ObtenirMsgSansParametre)
            .Aggregate((a, b) => $"{a}\n{b}");

        windowManager.ShowMessageBox(message, "Problèmes dans le formulaire", MessageBoxButton.OK,
            MessageBoxImage.Warning);
    }

    public void GererException(Exception exception)
    {
        if (exception is AggregateException aggregateException)
        {
            GererException(aggregateException);
            return;
        }

        string message = ObtenirMsgSansParametre(exception);

        windowManager.ShowMessageBox(message, "Problème dans le formulaire", MessageBoxButton.OK,
            MessageBoxImage.Warning);
    }
}