using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services;
using CineQuebec.Application.Records.Projections;
using CineQuebec.Domain.Interfaces.Entities.Projections;

namespace CineQuebec.Application.Services;

public class ProjectionQueryService(IUnitOfWorkFactory unitOfWorkFactory) : IProjectionQueryService
{
    public async Task<IEnumerable<ProjectionDto>> ObtenirProjectionsAVenirPourFilm(Guid idFilm)
    {
        using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();

        if (await unitOfWork.FilmRepository.ObtenirParIdAsync(idFilm) is null)
        {
            return [];
        }

        IEnumerable<IProjection> projections = (await unitOfWork.ProjectionRepository.ObtenirTousAsync(
            pf => pf.IdFilm == idFilm && pf.DateHeure >= DateTime.Now, iq => iq.OrderBy(pf => pf.DateHeure))).ToArray();
        IEnumerable<ISalle> salles =
            await unitOfWork.SalleRepository.ObtenirParIdsAsync(projections.Select(pf => pf.IdSalle));

        return projections.Select(pf =>
                pf.VersDto(null, salles.FirstOrDefault(s => s.Id == pf.IdSalle)?.VersDto()))
            .OrderBy(projection => projection.DateHeure);
    }
}