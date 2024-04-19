using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services;
using CineQuebec.Application.Services.Abstract;
using CineQuebec.Domain.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Films;

namespace CineQuebec.Application.Services;

public class CategorieFilmCreationService(IUnitOfWorkFactory unitOfWorkFactory)
    : ServiceAvecValidation, ICategorieFilmCreationService
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
        LeverAggregateExceptionAuBesoin(
            ValiderNomAffichage(nomAffichage),
            await ValiderCategorieFilmEstUnique(unitOfWork, nomAffichage)
        );
    }

    private static async Task<ICategorieFilm> CreerCategorie(IUnitOfWork unitOfWork, string nomAffichage)
    {
        CategorieFilm categorieFilm = new(nomAffichage);
        return await unitOfWork.CategorieFilmRepository.AjouterAsync(categorieFilm);
    }

    private static async Task<ArgumentException?> ValiderCategorieFilmEstUnique(IUnitOfWork unitOfWork,
        string nomAffichage)
    {
        string nomAffichageLower = nomAffichage.ToLowerInvariant();

        if (await unitOfWork.CategorieFilmRepository.ExisteAsync(c =>
                c.NomAffichage.ToLowerInvariant() == nomAffichageLower))
        {
            return new ArgumentException("Une catégorie de film avec le même nom d'affichage existe déjà.");
        }

        return null;
    }

    private static ArgumentException? ValiderNomAffichage(string nomAffichage)
    {
        return string.IsNullOrWhiteSpace(nomAffichage)
            ? new ArgumentException("Le nom d'affichage de la catégorie ne doit pas être vide.")
            : null;
    }
}