using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services;
using CineQuebec.Application.Services.Abstract;
using CineQuebec.Domain.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Films;

namespace CineQuebec.Application.Services;

public class RealisateurCreationService(IUnitOfWorkFactory unitOfWorkFactory)
    : ServiceAvecValidation, IRealisateurCreationService
{
    public async Task<Guid> CreerRealisateur(string prenom, string nom)
    {
        prenom = prenom.Trim();
        nom = nom.Trim();

        using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();

        await EffectuerValidations(unitOfWork, prenom, nom);
        IRealisateur realisateurAjoute = await CreerNouvRealisateur(unitOfWork, prenom, nom);

        await unitOfWork.SauvegarderAsync();

        return realisateurAjoute.Id;
    }

    private static async Task EffectuerValidations(IUnitOfWork unitOfWork, string prenom,
        string nom)
    {
        LeverAggregateExceptionAuBesoin(
            ValiderPrenom(prenom),
            ValiderNom(nom),
            await ValiderRealisateurEstUnique(unitOfWork, prenom, nom)
        );
    }

    private static async Task<IRealisateur> CreerNouvRealisateur(IUnitOfWork unitOfWork, string prenom,
        string nom)
    {
        Realisateur realisateur = new(prenom, nom);

        return await unitOfWork.RealisateurRepository.AjouterAsync(realisateur);
    }

    private static ArgumentException? ValiderNom(string nom)
    {
        return string.IsNullOrWhiteSpace(nom)
            ? new ArgumentException("Le nom de du realisateur ne doit pas être vide.")
            : null;
    }

    private static ArgumentException? ValiderPrenom(string prenom)
    {
        return string.IsNullOrWhiteSpace(prenom)
            ? new ArgumentException("Le prénom de du realisateur ne doit pas être vide.")
            : null;
    }

    private static async Task<ArgumentException?> ValiderRealisateurEstUnique(IUnitOfWork unitOfWork, string prenom,
        string nom)
    {
        string prenomLower = prenom.ToLowerInvariant();
        string nomLower = nom.ToLowerInvariant();

        if (await unitOfWork.RealisateurRepository.ExisteAsync(a =>
                a.Prenom.ToLowerInvariant() == prenomLower && a.Nom.ToLowerInvariant() == nomLower))
        {
            return new ArgumentException("Un realisateur avec le même prénom et nom existe déjà.");
        }

        return null;
    }
}