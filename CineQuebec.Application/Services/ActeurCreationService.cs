using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services;
using CineQuebec.Domain.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Films;

namespace CineQuebec.Application.Services;

public class ActeurCreationService(IUnitOfWorkFactory unitOfWorkFactory) : IActeurCreationService
{
    public async Task<Guid> CreerActeur(string prenom, string nom)
    {
        prenom = prenom.Trim();
        nom = nom.Trim();

        using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();

        IEnumerable<Exception> exceptions = await EffectuerValidations(unitOfWork, prenom, nom);

        if (exceptions.ToArray() is { Length: > 0 } innerExceptions)
        {
            throw new AggregateException("Des erreurs se sont produites lors de la validation des données.",
                innerExceptions);
        }

        Acteur acteur = new(prenom, nom);

        IActeur acteurAjoute = await unitOfWork.ActeurRepository.AjouterAsync(acteur);
        await unitOfWork.SauvegarderAsync();

        return acteurAjoute.Id;
    }

    private static async Task<IEnumerable<Exception>> EffectuerValidations(IUnitOfWork unitOfWork, string prenom,
        string nom)
    {
        List<Exception> exceptions = [];

        exceptions.AddRange(ValiderPrenom(prenom));
        exceptions.AddRange(ValiderNom(nom));
        exceptions.AddRange(await ValiderActeurEstUnique(unitOfWork, prenom, nom));

        return exceptions;
    }

    private static async Task<IEnumerable<Exception>> ValiderActeurEstUnique(IUnitOfWork unitOfWork, string prenom,
        string nom)
    {
        List<Exception> exceptions = [];

        string prenomLower = prenom.ToLowerInvariant();
        string nomLower = nom.ToLowerInvariant();

        if (await unitOfWork.ActeurRepository.ExisteAsync(a =>
                a.Prenom.ToLowerInvariant() == prenomLower && a.Nom.ToLowerInvariant() == nomLower))
        {
            exceptions.Add(new ArgumentException("Un acteur avec le même prénom et nom existe déjà."));
        }

        return exceptions;
    }

    private static IEnumerable<Exception> ValiderNom(string nom)
    {
        List<Exception> exceptions = [];

        if (string.IsNullOrWhiteSpace(nom))
        {
            exceptions.Add(new ArgumentException("Le nom de l'acteur ne doit pas être vide."));
        }

        return exceptions;
    }

    private static IEnumerable<Exception> ValiderPrenom(string prenom)
    {
        List<Exception> exceptions = [];

        if (string.IsNullOrWhiteSpace(prenom))
        {
            exceptions.Add(new ArgumentException("Le prénom de l'acteur ne doit pas être vide."));
        }

        return exceptions;
    }
}