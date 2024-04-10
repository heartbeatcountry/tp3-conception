using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services;
using CineQuebec.Application.Records.Films;
using CineQuebec.Domain.Interfaces.Entities.Films;

namespace CineQuebec.Application.Services;

public class CategorieFilmQueryService(IUnitOfWorkFactory unitOfWorkFactory) : ICategorieFilmQueryService
{
    public async Task<IEnumerable<CategorieFilmDto>> ObtenirToutes()
    {
        using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();

        IEnumerable<ICategorieFilm> categories = await unitOfWork.CategorieFilmRepository.ObtenirTousAsync();

        return categories.Select(categorie => categorie.VersDto());
    }
}