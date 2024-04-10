using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services;
using CineQuebec.Domain.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Films;

namespace CineQuebec.Application.Services;

public class CategorieFilmCreationService(IUnitOfWorkFactory unitOfWorkFactory) : ICategorieFilmCreationService
{
    public async Task<Guid> CreerCategorie(string nomAffichage)
    {
        nomAffichage = nomAffichage.Trim();

        using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();

        IEnumerable<Exception> exceptions = await EffectuerValidations(unitOfWork, nomAffichage);

        if (exceptions.ToArray() is { Length: > 0 } innerExceptions)
        {
            throw new AggregateException("Des erreurs se sont produites lors de la validation des données.",
                innerExceptions);
        }

        CategorieFilm categorieFilm = new CategorieFilm(nomAffichage);

        ICategorieFilm categorieFilmAjoute = await unitOfWork.CategorieFilmRepository.AjouterAsync(categorieFilm);
        await unitOfWork.SauvegarderAsync();

        return categorieFilmAjoute.Id;
    }

    private static async Task<IEnumerable<Exception>> EffectuerValidations(IUnitOfWork unitOfWork, string nomAffichage)
    {
        List<Exception> exceptions = [];

        exceptions.AddRange(ValiderNomAffichage(nomAffichage));
        exceptions.AddRange(await ValiderCategorieFilmEstUnique(unitOfWork, nomAffichage));

        return exceptions;
    }

    private static async Task<IEnumerable<Exception>> ValiderCategorieFilmEstUnique(IUnitOfWork unitOfWork,
        string nomAffichage)
    {
        List<Exception> exceptions = [];

        string nomAffichageLower = nomAffichage.ToLowerInvariant();

        if (await unitOfWork.CategorieFilmRepository.ExisteAsync(c =>
                c.NomAffichage.ToLowerInvariant() == nomAffichageLower))
        {
            exceptions.Add(new ArgumentException("Une catégorie de film avec le même nom d'affichage existe déjà."));
        }

        return exceptions;
    }

    private static IEnumerable<Exception> ValiderNomAffichage(string nomAffichage)
    {
        List<Exception> exceptions = [];

        if (string.IsNullOrWhiteSpace(nomAffichage))
        {
            exceptions.Add(new ArgumentException("Le nom d'affichage de la catégorie ne doit pas être vide."));
        }

        return exceptions;
    }
}