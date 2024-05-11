using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services.Fidelite;
using CineQuebec.Application.Records.Identity;
using CineQuebec.Domain.Interfaces.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Projections;
using CineQuebec.Domain.Interfaces.Entities.Utilisateur;

namespace CineQuebec.Application.Services.Fidelite;

public class UtilisateurFideliteQueryService(IUnitOfWorkFactory unitOfWorkFactory) : IUtilisateurFideliteQueryService
{
    public async Task<IEnumerable<UtilisateurDto>> ObtenirUtilisateursFideles(Guid idProjection)
    {
        using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();

        IProjection projection = await ObtenirProjection(unitOfWork, idProjection);
        IFilm film = await ObtenirFilm(unitOfWork, projection.IdFilm);

        return projection.EstAvantPremiere switch
        {
            true => await ObtenirUtilisateursFidelesAvantPremiere(unitOfWork, film),
            false => await ObtenirUtilisateursFidelesReprojection(unitOfWork, film)
        };
    }

    private static async Task<IProjection> ObtenirProjection(IUnitOfWork unitOfWork, Guid idProjection)
    {
        return await unitOfWork.ProjectionRepository.ObtenirParIdAsync(idProjection)
               ?? throw new KeyNotFoundException("La projection n'existe pas");
    }

    private static async Task<IFilm> ObtenirFilm(IUnitOfWork unitOfWork, Guid idFilm)
    {
        return await unitOfWork.FilmRepository.ObtenirParIdAsync(idFilm)
               ?? throw new KeyNotFoundException("Le film n'existe pas");
    }

    ///////////////////////////////////////////////////////////////////////////////////////
    // NOTE: Le code qui suit est absolument horrible: on charge la BD en entier en mémoire
    //       et on fait des boucles manuellement sur les enregistrements. Ça me chagrine
    //       beaucoup, mais je ne peux pas faire mieux car le plugin Entity Framework pour
    //       MongoDB ne supporte ni les relations, ni les joins. C'est insensé.

    private static async Task<IEnumerable<UtilisateurDto>> ObtenirUtilisateursFidelesAvantPremiere(
        IUnitOfWork unitOfWork, IFilm film)
    {
        IEnumerable<Guid> idsUtilisateursFideles = await ObtenirUtilisateursFideles(unitOfWork);
        List<UtilisateurDto> utilisateursFideles = [];

        foreach (Guid idUtilisateur in idsUtilisateursFideles)
        {
            IUtilisateur? utilisateur = await unitOfWork.UtilisateurRepository.ObtenirParIdAsync(idUtilisateur);

            if (utilisateur != null && (utilisateur.ActeursFavorisParId.Any(film.ActeursParId.Contains) ||
                                        utilisateur.RealisateursFavorisParId.Any(film.RealisateursParId.Contains)))
            {
                utilisateursFideles.Add(utilisateur.VersDto());
            }
        }

        return utilisateursFideles;
    }

    private static async Task<IEnumerable<UtilisateurDto>> ObtenirUtilisateursFidelesReprojection(
        IUnitOfWork unitOfWork, IFilm film)
    {
        IEnumerable<Guid> idsUtilisateursFideles = await ObtenirUtilisateursFideles(unitOfWork);
        List<UtilisateurDto> utilisateursFideles = [];

        foreach (Guid idUtilisateur in idsUtilisateursFideles)
        {
            IUtilisateur? utilisateur = await unitOfWork.UtilisateurRepository.ObtenirParIdAsync(idUtilisateur);

            if (utilisateur != null && utilisateur.CategoriesPrefereesParId.Contains(film.IdCategorie))
            {
                utilisateursFideles.Add(utilisateur.VersDto());
            }
        }

        return utilisateursFideles;
    }

    private static async Task<IEnumerable<Guid>> ObtenirUtilisateursFideles(IUnitOfWork unitOfWork)
    {
        IEnumerable<IBillet> billets = await unitOfWork.BilletRepository.ObtenirTousAsync();
        return billets.GroupBy(b => b.IdUtilisateur)
            .OrderByDescending(g => g.Count())
            .Select(g => g.Key);
    }
}