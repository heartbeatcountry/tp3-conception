using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services;
using CineQuebec.Application.Services.Abstract;
using CineQuebec.Domain.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Films;

namespace CineQuebec.Application.Services;

public class FilmCreationService(IUnitOfWorkFactory unitOfWorkFactory) : ServiceAvecValidation, IFilmCreationService
{
    public async Task<Guid> CreerFilm(string titre, string description, Guid categorie, DateTime
        dateDeSortieInternationale, IEnumerable<Guid> acteurs, IEnumerable<Guid> realisateurs, ushort duree)
    {
        titre = titre.Trim();
        description = description.Trim();
        Guid[] acteursParId = acteurs as Guid[] ?? acteurs.ToArray();
        Guid[] realisateursParId = realisateurs as Guid[] ?? realisateurs.ToArray();

        using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();

        await EffectuerValidations(unitOfWork, titre, description, categorie, dateDeSortieInternationale, acteursParId,
            realisateursParId, duree);

        IFilm filmCree = await CreerFilm(unitOfWork, titre, description, categorie, dateDeSortieInternationale,
            acteursParId, realisateursParId, duree);

        await unitOfWork.SauvegarderAsync();

        return filmCree.Id;
    }

    private static async Task EffectuerValidations(IUnitOfWork unitOfWork, string titre,
        string description, Guid categorie, DateTime dateDeSortieInternationale, IEnumerable<Guid> acteurs,
        IEnumerable<Guid> realisateurs, ushort duree)
    {
        LeverAggregateExceptionAuBesoin(
            ValiderTitre(titre),
            ValiderDescription(description),
            ValiderDuree(duree),
            ValiderDateSortieInternationale(dateDeSortieInternationale),
            await ValiderCategorieExiste(unitOfWork, categorie),
            await ValiderActeursExistent(unitOfWork, acteurs),
            await ValiderRealisateursExistent(unitOfWork, realisateurs),
            await ValiderFilmEstUnique(unitOfWork, titre, dateDeSortieInternationale.Year, duree)
        );
    }

    private static async Task<IFilm> CreerFilm(IUnitOfWork unitOfWork, string titre, string description, Guid categorie,
        DateTime dateDeSortieInternationale, IEnumerable<Guid> acteurs, IEnumerable<Guid> realisateurs, ushort duree)
    {
        Film film = new(titre, description, categorie, dateDeSortieInternationale, acteurs, realisateurs, duree);
        return await unitOfWork.FilmRepository.AjouterAsync(film);
    }

    private static async Task<IEnumerable<ArgumentException>> ValiderActeursExistent(IUnitOfWork unitOfWork,
        IEnumerable<Guid> acteurs)
    {
        List<ArgumentException> exceptions = [];

        foreach (Guid idActeur in acteurs)
        {
            if (await unitOfWork.ActeurRepository.ObtenirParIdAsync(idActeur) is null)
            {
                exceptions.Add(new ArgumentException($"L'acteur avec l'identifiant {idActeur} n'existe pas.",
                    nameof(acteurs)));
            }
        }

        return exceptions;
    }

    private static async Task<ArgumentException?> ValiderCategorieExiste(IUnitOfWork unitOfWork, Guid categorie)
    {
        return await unitOfWork.CategorieFilmRepository.ObtenirParIdAsync(categorie) is null
            ? new ArgumentException($"La catégorie de film avec l'identifiant {categorie} n'existe pas.",
                nameof(categorie))
            : null;
    }

    private static ArgumentOutOfRangeException? ValiderDateSortieInternationale(DateTime dateSortieInternationale)
    {
        return dateSortieInternationale <= DateTime.MinValue
            ? new ArgumentOutOfRangeException(nameof(dateSortieInternationale),
                $"La date de sortie internationale doit être supérieure à {DateOnly.MinValue}.")
            : null;
    }

    private static ArgumentException? ValiderDescription(string description)
    {
        return string.IsNullOrWhiteSpace(description)
            ? new ArgumentException("La description ne peut pas être vide.", nameof(description))
            : null;
    }

    private static ArgumentOutOfRangeException? ValiderDuree(ushort duree)
    {
        return duree == 0
            ? new ArgumentOutOfRangeException(nameof(duree), "Le film doit durer plus de 0 minutes.")
            : null;
    }

    private static async Task<ArgumentException?> ValiderFilmEstUnique(IUnitOfWork unitOfWork, string titre, int
        annee, ushort duree)
    {
        string titreLower = titre.ToLowerInvariant();

        if (await unitOfWork.FilmRepository.ExisteAsync(f =>
                f.Titre.ToLowerInvariant() == titreLower && f.DateSortieInternationale.Year == annee &&
                f.DureeEnMinutes == duree))
        {
            return new ArgumentException(
                "Un film avec le même titre, la même année de sortie et la même durée existe déjà.", nameof(titre));
        }

        return null;
    }

    private static async Task<IEnumerable<ArgumentException>> ValiderRealisateursExistent(IUnitOfWork unitOfWork,
        IEnumerable<Guid> realisateurs)
    {
        List<ArgumentException> exceptions = [];

        foreach (Guid idRealisateur in realisateurs)
        {
            if (await unitOfWork.RealisateurRepository.ObtenirParIdAsync(idRealisateur) is null)
            {
                exceptions.Add(new ArgumentException($"Le réalisateur avec l'identifiant {idRealisateur} n'existe pas.",
                    nameof(realisateurs)));
            }
        }

        return exceptions;
    }

    private static ArgumentException? ValiderTitre(string titre)
    {
        return string.IsNullOrWhiteSpace(titre)
            ? new ArgumentException("Le titre ne peut pas être vide.", nameof(titre))
            : null;
    }
}