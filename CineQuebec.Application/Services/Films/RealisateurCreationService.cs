using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services.Films;
using CineQuebec.Application.Services.Abstract;
using CineQuebec.Domain.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Films;

namespace CineQuebec.Application.Services.Films;

public class RealisateurCreationService(IUnitOfWorkFactory unitOfWorkFactory)
    : ServiceAvecValidation, IRealisateurCreationService
{
    public async Task<Guid> CreerRealisateur(string prenom, string nom)
    {
        prenom = prenom.Trim();
        nom = nom.Trim();

        using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();

        EffectuerValidations(unitOfWork, prenom, nom);
        IRealisateur realisateurAjoute = await CreerNouvRealisateur(unitOfWork, prenom, nom);

        await unitOfWork.SauvegarderAsync();

        return realisateurAjoute.Id;
    }

    private static void EffectuerValidations(IUnitOfWork unitOfWork, string prenom,
        string nom)
    {
        LeverAggregateExceptionAuBesoin(
            ValiderPrenom(prenom),
            ValiderNom(nom),
            ValiderRealisateurEstUnique(unitOfWork, prenom, nom)
        );
    }

    private static async Task<IRealisateur> CreerNouvRealisateur(IUnitOfWork unitOfWork, string prenom,
        string nom)
    {
        Realisateur realisateur = new(prenom, nom);

        return await unitOfWork.RealisateurRepository.AjouterAsync(realisateur);
    }

    private static IEnumerable<ArgumentException> ValiderNom(string nom)
    {
        if (string.IsNullOrWhiteSpace(nom))
        {
            yield return new ArgumentException("Le nom de du realisateur ne doit pas être vide.");
        }
    }

    private static IEnumerable<ArgumentException> ValiderPrenom(string prenom)
    {
        if (string.IsNullOrWhiteSpace(prenom))
        {
            yield return new ArgumentException("Le prénom de du realisateur ne doit pas être vide.");
        }
    }

    private static async IAsyncEnumerable<ArgumentException> ValiderRealisateurEstUnique(IUnitOfWork unitOfWork,
        string prenom,
        string nom)
    {
        string prenomLower = prenom.ToLowerInvariant();
        string nomLower = nom.ToLowerInvariant();

        if (await unitOfWork.RealisateurRepository.ExisteAsync(a =>
                a.Prenom.ToLowerInvariant() == prenomLower && a.Nom.ToLowerInvariant() == nomLower))
        {
            yield return new ArgumentException("Un realisateur avec le même prénom et nom existe déjà.");
        }
    }
}