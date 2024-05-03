using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services.Identity;
using CineQuebec.Application.Interfaces.Services.Projections;
using CineQuebec.Application.Records.Films;
using CineQuebec.Application.Records.Projections;
using CineQuebec.Domain.Interfaces.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Projections;

namespace CineQuebec.Application.Services.Projections;

public class BilletQueryService(
    IUnitOfWorkFactory unitOfWorkFactory,
    IUtilisateurAuthenticationService utilisateurAuthenticationService) : IBilletQueryService
{
    public async Task<IEnumerable<BilletDto>> ObtenirTous()
    {
        Guid idUtilisateur = utilisateurAuthenticationService.ObtenirIdUtilisateurConnecte();

        using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();
        IBillet[] billets = (await unitOfWork.BilletRepository.ObtenirTousAsync(b => b.IdUtilisateur == idUtilisateur))
            .ToArray();
        BilletDto[] billetDtos = new BilletDto[billets.Length];

        for (int i = 0; i < billets.Length; i++)
        {
            IBillet billet = billets[i];
            IProjection projection = await unitOfWork.ProjectionRepository.ObtenirParIdAsync(billet.IdProjection) ??
                                     throw new KeyNotFoundException(
                                         $"Impossible de trouver la projection pour le billet {billet}");
            IFilm film = await unitOfWork.FilmRepository.ObtenirParIdAsync(projection.IdFilm) ??
                         throw new KeyNotFoundException(
                             $"Impossible de trouver le film pour la projection {projection}");

            FilmDto filmDto = film.VersDto(null, [], []);
            ProjectionDto projectionDto = projection.VersDto(filmDto, null);

            billetDtos[i] = billet.VersDto(projectionDto, null);
        }

        return billetDtos;
    }
}