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
        await EffectuerValidations(unitOfWork, prenom, nom);

        IActeur acteurAjoute = await CreerActeur(unitOfWork, prenom, nom);

        await unitOfWork.SauvegarderAsync();

        return acteurAjoute.Id;
    }

    private static async Task EffectuerValidations(IUnitOfWork unitOfWork, string prenom,
        string nom)
    {
        LeverAggregateExceptionAuBesoin(
            ValiderPrenom(prenom),
            ValiderNom(nom),
            await ValiderActeurEstUnique(unitOfWork, prenom, nom)
        );
    }

    private static async Task<IActeur> CreerActeur(IUnitOfWork unitOfWork, string prenom, string nom)
    {
        Acteur acteur = new(prenom, nom);
        return await unitOfWork.ActeurRepository.AjouterAsync(acteur);
    }

    private static async Task<ArgumentException?> ValiderActeurEstUnique(IUnitOfWork unitOfWork, string prenom,
        string nom)
    {
        string prenomLower = prenom.ToLowerInvariant();
        string nomLower = nom.ToLowerInvariant();

        if (await unitOfWork.ActeurRepository.ExisteAsync(a =>
                a.Prenom.ToLowerInvariant() == prenomLower && a.Nom.ToLowerInvariant() == nomLower))
        {
            return new ArgumentException("Un acteur avec le même prénom et nom existe déjà.");
        }

        return null;
    }

    private static ArgumentException? ValiderNom(string nom)
    {
        return string.IsNullOrWhiteSpace(nom)
            ? new ArgumentException("Le nom de l'acteur ne doit pas être vide.")
            : null;
    }

    private static ArgumentException? ValiderPrenom(string prenom)
    {
        return string.IsNullOrWhiteSpace(prenom)
            ? new ArgumentException("Le prénom de l'acteur ne doit pas être vide.")
            : null;
    }
}