using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services;
using CineQuebec.Application.Services.Abstract;
using CineQuebec.Domain.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Films;

namespace CineQuebec.Application.Services;

public class ActeurCreationService(IUnitOfWorkFactory unitOfWorkFactory)
    : ServiceAvecValidation, IActeurCreationService
{
    public async Task<Guid> CreerActeur(string prenom, string nom)
    {
        using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();

        (prenom, nom) = (prenom.Trim(), nom.Trim());
        EffectuerValidations(unitOfWork, prenom, nom);

        IActeur acteurAjoute = await CreerActeur(unitOfWork, prenom, nom);

        await unitOfWork.SauvegarderAsync();

        return acteurAjoute.Id;
    }

    private static void EffectuerValidations(IUnitOfWork unitOfWork, string prenom,
        string nom)
    {
        LeverAggregateExceptionAuBesoin(
            ValiderPrenom(prenom),
            ValiderNom(nom),
            ValiderActeurEstUnique(unitOfWork, prenom, nom)
        );
    }

    private static async Task<IActeur> CreerActeur(IUnitOfWork unitOfWork, string prenom, string nom)
    {
        Acteur acteur = new(prenom, nom);
        return await unitOfWork.ActeurRepository.AjouterAsync(acteur);
    }

    private static async IAsyncEnumerable<ArgumentException> ValiderActeurEstUnique(IUnitOfWork unitOfWork,
        string prenom,
        string nom)
    {
        string prenomLower = prenom.ToLowerInvariant();
        string nomLower = nom.ToLowerInvariant();

        if (await unitOfWork.ActeurRepository.ExisteAsync(a =>
                a.Prenom.ToLowerInvariant() == prenomLower && a.Nom.ToLowerInvariant() == nomLower))
        {
            yield return new ArgumentException("Un acteur avec le même prénom et nom existe déjà.");
        }
    }

    private static IEnumerable<ArgumentException> ValiderNom(string nom)
    {
        if (string.IsNullOrWhiteSpace(nom))
        {
            yield return new ArgumentException("Le nom de l'acteur ne doit pas être vide.");
        }
    }

    private static IEnumerable<ArgumentException> ValiderPrenom(string prenom)
    {
        if (string.IsNullOrWhiteSpace(prenom))
        {
            yield return new ArgumentException("Le prénom de l'acteur ne doit pas être vide.");
        }
    }
}