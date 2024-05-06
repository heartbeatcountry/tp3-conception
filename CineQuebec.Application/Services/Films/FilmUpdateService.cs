using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services.Films;
using CineQuebec.Application.Services.Abstract;
using CineQuebec.Domain.Interfaces.Entities.Films;

namespace CineQuebec.Application.Services.Films;

public class FilmUpdateService(IUnitOfWorkFactory unitOfWorkFactory) : ServiceAvecValidation, IFilmUpdateService
{
    public async Task ModifierFilm(Guid idFilm, string titre, string description, Guid categorie, DateTime
        dateDeSortieInternationale, IEnumerable<Guid> acteurs, IEnumerable<Guid> realisateurs, ushort duree)
    {
        titre = titre.Trim();
        description = description.Trim();
        Guid[] acteursParId = acteurs as Guid[] ?? acteurs.ToArray();
        Guid[] realisateursParId = realisateurs as Guid[] ?? realisateurs.ToArray();

        using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();

        EffectuerValidations(unitOfWork, idFilm, titre, description,
            categorie, dateDeSortieInternationale, acteursParId, realisateursParId, duree);

        await ModifierFilm(unitOfWork, idFilm, titre, description, categorie, dateDeSortieInternationale,
            acteursParId, realisateursParId, duree);

        await unitOfWork.SauvegarderAsync();
    }

    private static void EffectuerValidations(IUnitOfWork unitOfWork, Guid id,
        string titre, string description, Guid categorie, DateTime dateDeSortieInternationale,
        IEnumerable<Guid> acteurs, IEnumerable<Guid> realisateurs, ushort duree)
    {
        LeverAggregateExceptionAuBesoin(
            ValiderFilmExiste(unitOfWork, id),
            ValiderTitre(titre),
            ValiderDescription(description),
            ValiderDuree(duree),
            ValiderDateSortieInternationale(dateDeSortieInternationale),
            ValiderCategorieExiste(unitOfWork, categorie),
            ValiderActeursExistent(unitOfWork, acteurs),
            ValiderRealisateursExistent(unitOfWork, realisateurs),
            ValiderFilmEstUnique(unitOfWork, id, titre, dateDeSortieInternationale.Year, duree)
        );
    }

    private static async Task ModifierFilm(IUnitOfWork unitOfWork, Guid id, string titre, string description,
        Guid categorie, DateTime dateDeSortieInternationale, IEnumerable<Guid> acteursParId,
        IEnumerable<Guid> realisateursParId, ushort duree)
    {
        IFilm film = (await unitOfWork.FilmRepository.ObtenirParIdAsync(id))!;

        film.SetTitre(titre);
        film.SetDescription(description);
        film.SetCategorie(categorie);
        film.SetDateSortieInternationale(dateDeSortieInternationale);
        film.SetActeursParId(acteursParId);
        film.SetRealisateursParId(realisateursParId);
        film.SetDureeEnMinutes(duree);

        unitOfWork.FilmRepository.Modifier(film);
    }

    private static async IAsyncEnumerable<ArgumentException> ValiderActeursExistent(IUnitOfWork unitOfWork,
        IEnumerable<Guid> acteurs)
    {
        foreach (Guid idActeur in acteurs)
        {
            if (await unitOfWork.ActeurRepository.ObtenirParIdAsync(idActeur) is null)
            {
                yield return new ArgumentException($"L'acteur avec l'identifiant {idActeur} n'existe pas.",
                    nameof(acteurs));
            }
        }
    }

    private static async IAsyncEnumerable<ArgumentException> ValiderCategorieExiste(IUnitOfWork unitOfWork,
        Guid categorie)
    {
        if (await unitOfWork.CategorieFilmRepository.ObtenirParIdAsync(categorie) is null)
        {
            yield return new ArgumentException($"La catégorie de film avec l'identifiant {categorie} n'existe pas.",
                nameof(categorie));
        }
    }

    private static IEnumerable<ArgumentOutOfRangeException> ValiderDateSortieInternationale(
        DateTime dateSortieInternationale)
    {
        if (dateSortieInternationale <= DateTime.MinValue)
        {
            yield return new ArgumentOutOfRangeException(nameof(dateSortieInternationale),
                $"La date de sortie internationale doit être supérieure à {DateOnly.MinValue}.");
        }
    }

    private static IEnumerable<ArgumentException> ValiderDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            yield return new ArgumentException("La description ne peut pas être vide.", nameof(description));
        }
    }

    private static IEnumerable<ArgumentOutOfRangeException> ValiderDuree(ushort duree)
    {
        if (duree == 0)
        {
            yield return new ArgumentOutOfRangeException(nameof(duree), "Le film doit durer plus de 0 minutes.");
        }
    }

    private static async IAsyncEnumerable<ArgumentException> ValiderFilmEstUnique(IUnitOfWork unitOfWork, Guid id,
        string titre, int annee, ushort duree)
    {
        string titreLower = titre.ToLowerInvariant();

        if (await unitOfWork.FilmRepository.ExisteAsync(f =>
                f.Id != id && f.Titre.ToLowerInvariant() == titreLower && f.DateSortieInternationale.Year == annee &&
                f.DureeEnMinutes == duree))
        {
            yield return new ArgumentException(
                "Un film avec le même titre, la même année de sortie et la même durée existe déjà.", nameof(titre));
        }
    }

    private static async IAsyncEnumerable<ArgumentException> ValiderFilmExiste(IUnitOfWork unitOfWork, Guid id)
    {
        if (await unitOfWork.FilmRepository.ObtenirParIdAsync(id) is null)
        {
            yield return new ArgumentException($"Le film avec l'identifiant {id} n'existe pas.",
                nameof(id));
        }
    }

    private static async IAsyncEnumerable<ArgumentException> ValiderRealisateursExistent(IUnitOfWork unitOfWork,
        IEnumerable<Guid> realisateurs)
    {
        foreach (Guid idRealisateur in realisateurs)
        {
            if (await unitOfWork.RealisateurRepository.ObtenirParIdAsync(idRealisateur) is null)
            {
                yield return new ArgumentException($"Le réalisateur avec l'identifiant {idRealisateur} n'existe pas.",
                    nameof(realisateurs));
            }
        }
    }

    private static IEnumerable<ArgumentException> ValiderTitre(string titre)
    {
        if (string.IsNullOrWhiteSpace(titre))
        {
            yield return new ArgumentException("Le titre ne peut pas être vide.", nameof(titre));
        }
    }
}