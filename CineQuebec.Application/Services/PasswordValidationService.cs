using System.Globalization;
using System.Reflection;
using System.Security;
using System.Text;

using CineQuebec.Application.Interfaces.Services;
using CineQuebec.Application.Services.Abstract;

namespace CineQuebec.Application.Services;

public class PasswordValidationService : ServiceAvecValidation, IPasswordValidationService
{
    // Lecture de chevet: https://blog.codinghorror.com/password-rules-are-bullshit/

    private const string NomFichierMotsDePasseLesPlusCommuns = "MotsDePasseLesPlusCommuns.txt";
    public const byte LongueurMinimaleMdp = 10;
    public const byte LongueurMaximaleMdp = 128;
    public const byte NbCaracteresUniquesMin = 5;
    private static readonly HashSet<string> MotsDePasseLesPlusCommuns = [.. LireFichierMotsDePasseLesPlusCommuns()];

    public void ValiderMdpEstSecuritaire(string mdp)
    {
        string trimmedMdp = mdp.Trim();

        LeverAggregateExceptionAuBesoin(
            ValiderLongueurMdp(trimmedMdp),
            ValiderMdpCommun(trimmedMdp),
            ValiderCaracteresUniques(trimmedMdp)
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
        if (MotsDePasseLesPlusCommuns.Contains(NormaliserMdp(trimmedMdp)))
        {
            yield return new SecurityException(
                "Le mot de passe est trop commun. Veuillez utiliser un mot de passe unique.");
        }
    }

    private static IEnumerable<SecurityException> ValiderCaracteresUniques(string trimmedMdp)
    {
        if (CalculerNbCaracteresUniques(trimmedMdp) < NbCaracteresUniquesMin)
        {
            yield return new SecurityException(
                $"Le mot de passe doit contenir au moins {NbCaracteresUniquesMin} caractères uniques.");
        }
    }

    private static IEnumerable<string> LireFichierMotsDePasseLesPlusCommuns()
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        string resourceName = assembly.GetManifestResourceNames()
            .First(res => res.EndsWith(NomFichierMotsDePasseLesPlusCommuns));
        using Stream stream = assembly.GetManifestResourceStream(resourceName)!;
        using StreamReader reader = new(stream);

        while (!reader.EndOfStream && reader.ReadLine()?.Trim() is { Length: > 0 } mdp)
        {
            if (mdp.Length >= LongueurMinimaleMdp && CalculerNbCaracteresUniques(mdp) >= NbCaracteresUniquesMin)
            {
                yield return NormaliserMdp(mdp);
            }
        }
    }

    private static string NormaliserMdp(string mdp)
    {
        // Ce code peut être difficile à lire, mais en gros, il retire les accents et les caractères spéciaux
        // d'un mot de passe, pour le comparer à une liste de mots de passe communs. Ainsi, véronica et veronica
        // seraient considérés comme identiques lors de la vérification d'unicité.

        return string.Concat(mdp.ToLowerInvariant()
                .Normalize(NormalizationForm.FormD)
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark))
            .Normalize(NormalizationForm.FormC);
    }

    private static int CalculerNbCaracteresUniques(string trimmedMdp)
    {
        HashSet<string> textElementsUniques = [];
        TextElementEnumerator textElements = StringInfo.GetTextElementEnumerator(trimmedMdp);

        while (textElements.MoveNext())
        {
            textElementsUniques.Add(textElements.GetTextElement());
        }

        return textElementsUniques.Count;
    }
}