using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services;
using CineQuebec.Application.Records.Projections;
using CineQuebec.Domain.Interfaces.Entities.Projections;

namespace CineQuebec.Application.Services;

public class SalleQueryService(IUnitOfWorkFactory unitOfWorkFactory) : ISalleQueryService
{
    public async Task<IEnumerable<SalleDto>> ObtenirToutes()
    {
        using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();

        IEnumerable<ISalle> salles = await unitOfWork.SalleRepository.ObtenirTousAsync();

        return salles.Select(s => s.VersDto());
    }
}