using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services.Films;
using CineQuebec.Application.Services.Abstract;
using CineQuebec.Domain.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Films;

namespace CineQuebec.Application.Services.Films;

public class CategorieFilmCreationService(IUnitOfWorkFactory unitOfWorkFactory)
    : ServiceAvecValidation, ICategorieFilmCreationService
{
    public async Task<Guid> CreerCategorie(string nomAffichage)
    {
        using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();

        nomAffichage = nomAffichage.Trim();
        EffectuerValidations(unitOfWork, nomAffichage);

        ICategorieFilm categorieFilmAjoute = await CreerCategorie(unitOfWork, nomAffichage);

        await unitOfWork.SauvegarderAsync();

        return categorieFilmAjoute.Id;
    }

    private static void EffectuerValidations(IUnitOfWork unitOfWork, string nomAffichage)
    {
        LeverAggregateExceptionAuBesoin(
            ValiderNomAffichage(nomAffichage),
            ValiderCategorieFilmEstUnique(unitOfWork, nomAffichage)
        );
    }

    private static async Task<ICategorieFilm> CreerCategorie(IUnitOfWork unitOfWork, string nomAffichage)
    {
        CategorieFilm categorieFilm = new(nomAffichage);
        return await unitOfWork.CategorieFilmRepository.AjouterAsync(categorieFilm);
    }

    private static async IAsyncEnumerable<ArgumentException?> ValiderCategorieFilmEstUnique(IUnitOfWork unitOfWork,
        string nomAffichage)
    {
        string nomAffichageLower = nomAffichage.ToLowerInvariant();

        if (await unitOfWork.CategorieFilmRepository.ExisteAsync(c =>
                c.NomAffichage.ToLowerInvariant() == nomAffichageLower))
        {
            yield return new ArgumentException("Une catégorie de film avec le même nom d'affichage existe déjà.");
        }
    }

    private static IEnumerable<ArgumentException> ValiderNomAffichage(string nomAffichage)
    {
        if (string.IsNullOrWhiteSpace(nomAffichage))
        {
            yield return new ArgumentException("Le nom d'affichage de la catégorie ne doit pas être vide.");
        }
    }
}