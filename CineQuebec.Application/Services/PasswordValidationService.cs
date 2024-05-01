using System.Globalization;
using System.Reflection;
using System.Security;

using CineQuebec.Application.Interfaces.Services;
using CineQuebec.Application.Services.Abstract;

namespace CineQuebec.Application.Services;

public class PasswordValidationService : ServiceAvecValidation, IPasswordValidationService
{
    // Lecture de chevet: https://blog.codinghorror.com/password-rules-are-bullshit/

    private const string NomFichierMotsDePasseLesPlusCommuns = "MotsDePasseLesPlusCommuns.txt";
    public const byte LongueurMinimaleMdp = 10;
    public const byte LongueurMaximaleMdp = 128;
    private static readonly HashSet<string> MotsDePasseLesPlusCommuns = [.. LireFichierMotsDePasseLesPlusCommuns()];

    public void ValiderMdpEstSecuritaire(string mdp)
    {
        string trimmedMdp = mdp.Trim();

        LeverAggregateExceptionAuBesoin(
            ValiderLongueurMdp(trimmedMdp),
            ValiderMdpCommun(trimmedMdp)
        );
    }

    private static IEnumerable<SecurityException> ValiderLongueurMdp(string trimmedMdp)
    {
        StringInfo mdpStringInfo = new(trimmedMdp);

        // On valide le nombre de caractères Unicode, plutôt que le nombre d'octets:
        switch (mdpStringInfo.LengthInTextElements)
        {
            case < LongueurMinimaleMdp:
                yield return new SecurityException(
                    $"Le mot de passe doit contenir au moins {LongueurMinimaleMdp} caractères.");
                break;
            case > LongueurMaximaleMdp:
                yield return new SecurityException(
                    $"Le mot de passe doit contenir au plus {LongueurMaximaleMdp} caractères.");
                break;
        }
    }

    private static IEnumerable<SecurityException> ValiderMdpCommun(string trimmedMdp)
    {
        if (MotsDePasseLesPlusCommuns.Contains(trimmedMdp.ToLowerInvariant()))
        {
            yield return new SecurityException(
                "Le mot de passe est trop commun. Veuillez utiliser un mot de passe unique.");
        }
    }

    private static IEnumerable<string> LireFichierMotsDePasseLesPlusCommuns()
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        string resourceName = assembly.GetManifestResourceNames()
            .First(res => res.EndsWith(NomFichierMotsDePasseLesPlusCommuns));
        using Stream stream = assembly.GetManifestResourceStream(resourceName)!;
        using StreamReader reader = new(stream);

        while (!reader.EndOfStream && reader.ReadLine()?.Trim() is { Length: > 0 } ligne)
        {
            yield return ligne.ToLowerInvariant();
        }
    }
}