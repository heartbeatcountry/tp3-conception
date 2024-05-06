using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services.Films;
using CineQuebec.Application.Records.Films;
using CineQuebec.Domain.Interfaces.Entities.Films;

namespace CineQuebec.Application.Services.Films;

public class CategorieFilmQueryService(IUnitOfWorkFactory unitOfWorkFactory) : ICategorieFilmQueryService
{
    public async Task<IEnumerable<CategorieFilmDto>> ObtenirToutes()
    {
        using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();

        IEnumerable<ICategorieFilm> categories = await unitOfWork.CategorieFilmRepository.ObtenirTousAsync();

        return categories.Select(categorie => categorie.VersDto())
            .OrderBy(categorie => categorie.NomAffichage);
    }
}