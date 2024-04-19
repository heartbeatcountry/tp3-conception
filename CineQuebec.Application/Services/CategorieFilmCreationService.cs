using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services;
using CineQuebec.Domain.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Films;

namespace CineQuebec.Application.Services;

public class CategorieFilmCreationService(IUnitOfWorkFactory unitOfWorkFactory) : ICategorieFilmCreationService
{
    public async Task<Guid> CreerCategorie(string nomAffichage)
    {
        using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();

        nomAffichage = nomAffichage.Trim();
        await EffectuerValidations(unitOfWork, nomAffichage);

        ICategorieFilm categorieFilmAjoute = await CreerCategorie(unitOfWork, nomAffichage);

        await unitOfWork.SauvegarderAsync();

        return categorieFilmAjoute.Id;
    }

    private static async Task EffectuerValidations(IUnitOfWork unitOfWork, string nomAffichage)
    {
        List<Exception> exceptions = [];

        exceptions.AddRange(ValiderNomAffichage(nomAffichage));
        exceptions.AddRange(await ValiderCategorieFilmEstUnique(unitOfWork, nomAffichage));

        if (exceptions.ToArray() is { Length: > 0 } innerExceptions)
        {
            throw new AggregateException("Des erreurs se sont produites lors de la validation des données.",
                innerExceptions);
        }
    }

    private static async Task<ICategorieFilm> CreerCategorie(IUnitOfWork unitOfWork, string nomAffichage)
    {
        CategorieFilm categorieFilm = new(nomAffichage);
        return await unitOfWork.CategorieFilmRepository.AjouterAsync(categorieFilm);
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