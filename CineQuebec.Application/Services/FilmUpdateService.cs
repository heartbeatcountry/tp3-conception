using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services;
using CineQuebec.Domain.Interfaces.Entities.Films;

namespace CineQuebec.Application.Services;

public class FilmUpdateService(IUnitOfWorkFactory unitOfWorkFactory) : IFilmUpdateService
{
    private static async Task<IEnumerable<Exception>> EffectuerValidations(IUnitOfWork unitOfWork, Guid id,
        string titre, string description, Guid categorie, DateTime dateDeSortieInternationale,
        IEnumerable<Guid> acteurs, IEnumerable<Guid> realisateurs, ushort duree)
    {
        List<Exception> exceptions = [];

        exceptions.AddRange(await ValiderFilmExiste(unitOfWork, id));
        exceptions.AddRange(ValiderTitre(titre));
        exceptions.AddRange(ValiderDescription(description));
        exceptions.AddRange(ValiderDuree(duree));
        exceptions.AddRange(ValiderDateSortieInternationale(dateDeSortieInternationale));
        exceptions.AddRange(await ValiderCategorieExiste(unitOfWork, categorie));
        exceptions.AddRange(await ValiderActeursExistent(unitOfWork, acteurs));
        exceptions.AddRange(await ValiderRealisateursExistent(unitOfWork, realisateurs));
        exceptions.AddRange(await ValiderFilmEstUnique(unitOfWork, id, titre, dateDeSortieInternationale.Year, duree));

        return exceptions;
    }

    public async Task ModifierFilm(Guid idFilm, string titre, string description, Guid categorie, DateTime
        dateDeSortieInternationale, IEnumerable<Guid> acteurs, IEnumerable<Guid> realisateurs, ushort duree)
    {
        titre = titre.Trim();
        description = description.Trim();
        Guid[] acteursParId = acteurs as Guid[] ?? acteurs.ToArray();
        Guid[] realisateursParId = realisateurs as Guid[] ?? realisateurs.ToArray();

        using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();

        IEnumerable<Exception> exceptions = await EffectuerValidations(unitOfWork, idFilm, titre, description,
            categorie, dateDeSortieInternationale, acteursParId, realisateursParId, duree);

        if (exceptions.ToArray() is { Length: > 0 } innerExceptions)
        {
            throw new AggregateException("Des erreurs se sont produites lors de la validation des données.",
                innerExceptions);
        }

        IFilm film = (await unitOfWork.FilmRepository.ObtenirParIdAsync(idFilm))!;

        film.SetTitre(titre);
        film.SetDescription(description);
        film.SetCategorie(categorie);
        film.SetDateSortieInternationale(dateDeSortieInternationale);
        film.SetActeursParId(acteursParId);
        film.SetRealisateursParId(realisateursParId);
        film.SetDureeEnMinutes(duree);

        unitOfWork.FilmRepository.Modifier(film);
        await unitOfWork.SauvegarderAsync();
    }

    private static async Task<IEnumerable<Exception>> ValiderActeursExistent(IUnitOfWork unitOfWork,
        IEnumerable<Guid> acteurs)
    {
        List<Exception> exceptions = [];

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

    private static async Task<IEnumerable<Exception>> ValiderCategorieExiste(IUnitOfWork unitOfWork, Guid categorie)
    {
        List<Exception> exceptions = [];

        if (await unitOfWork.CategorieFilmRepository.ObtenirParIdAsync(categorie) is null)
        {
            exceptions.Add(new ArgumentException($"La catégorie de film avec l'identifiant {categorie} n'existe pas.",
                nameof(categorie)));
        }

        return exceptions;
    }

    private static IEnumerable<Exception> ValiderDateSortieInternationale(DateTime dateSortieInternationale)
    {
        List<Exception> exceptions = [];

        if (dateSortieInternationale <= DateTime.MinValue)
        {
            exceptions.Add(new ArgumentOutOfRangeException(nameof(dateSortieInternationale),
                $"La date de sortie internationale doit être supérieure à {DateOnly.MinValue}."));
        }

        return exceptions;
    }

    private static IEnumerable<Exception> ValiderDescription(string description)
    {
        List<Exception> exceptions = [];

        if (string.IsNullOrWhiteSpace(description))
        {
            exceptions.Add(new ArgumentException("La description ne peut pas être vide.", nameof(description)));
        }

        return exceptions;
    }

    private static IEnumerable<Exception> ValiderDuree(ushort duree)
    {
        List<Exception> exceptions = [];

        if (duree == 0)
        {
            exceptions.Add(new ArgumentOutOfRangeException(nameof(duree), "Le film doit durer plus de 0 minutes."));
        }

        return exceptions;
    }

    private static async Task<IEnumerable<Exception>> ValiderFilmEstUnique(IUnitOfWork unitOfWork, Guid id,
        string titre, int annee, ushort duree)
    {
        List<Exception> exceptions = [];

        string titreLower = titre.ToLowerInvariant();

        if (await unitOfWork.FilmRepository.ExisteAsync(f =>
                f.Id != id && f.Titre.ToLowerInvariant() == titreLower && f.DateSortieInternationale.Year == annee &&
                f.DureeEnMinutes == duree))
        {
            exceptions.Add(new ArgumentException(
                "Un film avec le même titre, la même année de sortie et la même durée existe déjà.", nameof(titre)));
        }

        return exceptions;
    }

    private static async Task<IEnumerable<Exception>> ValiderFilmExiste(IUnitOfWork unitOfWork, Guid id)
    {
        List<Exception> exceptions = [];

        if (await unitOfWork.FilmRepository.ObtenirParIdAsync(id) is null)
        {
            exceptions.Add(new ArgumentException($"Le film avec l'identifiant {id} n'existe pas.",
                nameof(id)));
        }

        return exceptions;
    }

    private static async Task<IEnumerable<Exception>> ValiderRealisateursExistent(IUnitOfWork unitOfWork,
        IEnumerable<Guid> realisateurs)
    {
        List<Exception> exceptions = [];

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

    private static IEnumerable<Exception> ValiderTitre(string titre)
    {
        List<Exception> exceptions = [];

        if (string.IsNullOrWhiteSpace(titre))
        {
            exceptions.Add(new ArgumentException("Le titre ne peut pas être vide.", nameof(titre)));
        }

        return exceptions;
    }
}