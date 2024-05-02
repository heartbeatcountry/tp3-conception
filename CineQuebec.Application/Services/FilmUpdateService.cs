using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services;
using CineQuebec.Application.Services.Abstract;
using CineQuebec.Domain.Interfaces.Entities.Films;

namespace CineQuebec.Application.Services;

public class FilmUpdateService(IUnitOfWorkFactory unitOfWorkFactory) : ServiceAvecValidation, IFilmUpdateService
{
<<<<<<< HEAD
    public async Task ModifierFilm(Guid idFilm, string titre, string description, Guid categorie, DateTime
        dateDeSortieInternationale, IEnumerable<Guid> acteurs, IEnumerable<Guid> realisateurs, ushort duree)
=======
    private static async Task<IEnumerable<Exception>> EffectuerValidations(IUnitOfWork unitOfWork, Guid id,
        string titre, string description, Guid categorie, DateTime dateDeSortieInternationale,
        IEnumerable<Guid> acteurs, IEnumerable<Guid> realisateurs, ushort duree )
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
        dateDeSortieInternationale, IEnumerable<Guid> acteurs, IEnumerable<Guid> realisateurs, ushort duree )
>>>>>>> 8a5e946 (Correction en lien avec commentaires du pull request)
    {
        titre = titre.Trim();
        description = description.Trim();
        Guid[] acteursParId = acteurs as Guid[] ?? acteurs.ToArray();
        Guid[] realisateursParId = realisateurs as Guid[] ?? realisateurs.ToArray();

        using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();

<<<<<<< HEAD
        await EffectuerValidations(unitOfWork, idFilm, titre, description,
=======
        IEnumerable<Exception> exceptions = await EffectuerValidations(unitOfWork, idFilm, titre, description,
>>>>>>> 8a5e946 (Correction en lien avec commentaires du pull request)
            categorie, dateDeSortieInternationale, acteursParId, realisateursParId, duree);

        await ModifierFilm(unitOfWork, idFilm, titre, description, categorie, dateDeSortieInternationale,
            acteursParId, realisateursParId, duree);

        await unitOfWork.SauvegarderAsync();
    }

    private static async Task EffectuerValidations(IUnitOfWork unitOfWork, Guid id,
        string titre, string description, Guid categorie, DateTime dateDeSortieInternationale,
        IEnumerable<Guid> acteurs, IEnumerable<Guid> realisateurs, ushort duree)
    {
        LeverAggregateExceptionAuBesoin(
            await ValiderFilmExiste(unitOfWork, id),
            ValiderTitre(titre),
            ValiderDescription(description),
            ValiderDuree(duree),
            ValiderDateSortieInternationale(dateDeSortieInternationale),
            await ValiderCategorieExiste(unitOfWork, categorie),
            await ValiderActeursExistent(unitOfWork, acteurs),
            await ValiderRealisateursExistent(unitOfWork, realisateurs),
            await ValiderFilmEstUnique(unitOfWork, id, titre, dateDeSortieInternationale.Year, duree)
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
<<<<<<< HEAD
=======

>>>>>>> 8a5e946 (Correction en lien avec commentaires du pull request)

        unitOfWork.FilmRepository.Modifier(film);
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

    private static async Task<ArgumentException?> ValiderFilmEstUnique(IUnitOfWork unitOfWork, Guid id,
        string titre, int annee, ushort duree)
    {
        List<Exception> exceptions = [];

        string titreLower = titre.ToLowerInvariant();

        if (await unitOfWork.FilmRepository.ExisteAsync(f =>
                f.Id != id && f.Titre.ToLowerInvariant() == titreLower && f.DateSortieInternationale.Year == annee &&
                f.DureeEnMinutes == duree))
        {
            return new ArgumentException(
                "Un film avec le même titre, la même année de sortie et la même durée existe déjà.", nameof(titre));
        }

        return null;
    }

    private static async Task<ArgumentException?> ValiderFilmExiste(IUnitOfWork unitOfWork, Guid id)
    {
        if (await unitOfWork.FilmRepository.ObtenirParIdAsync(id) is null)
        {
            return new ArgumentException($"Le film avec l'identifiant {id} n'existe pas.",
                nameof(id));
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
<<<<<<< HEAD
        return string.IsNullOrWhiteSpace(titre)
            ? new ArgumentException("Le titre ne peut pas être vide.", nameof(titre))
            : null;
    }
=======
        List<Exception> exceptions = [];

        if (string.IsNullOrWhiteSpace(titre))
        {
            exceptions.Add(new ArgumentException("Le titre ne peut pas être vide.", nameof(titre)));
        }

        return exceptions;
    }

>>>>>>> 8a5e946 (Correction en lien avec commentaires du pull request)
}