using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services;
using CineQuebec.Domain.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Films;

namespace CineQuebec.Application.Services;

public class FilmCreationService(IUnitOfWorkFactory unitOfWorkFactory) : IFilmCreationService
{
    public async Task<Guid> CreerFilm(string titre, string description, Guid categorie, DateTime
        dateDeSortieInternationale, IEnumerable<Guid> acteurs, IEnumerable<Guid> realisateurs, ushort duree)
    {
        Guid[] acteursParId = acteurs as Guid[] ?? acteurs.ToArray();
        Guid[] realisateursParId = realisateurs as Guid[] ?? realisateurs.ToArray();

        using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();

        var exceptions = await EffectuerValidations(unitOfWork, titre, description, categorie,
            dateDeSortieInternationale, acteursParId, realisateursParId, duree);

        if (exceptions.ToArray() is { Length: > 0 } innerExceptions)
        {
            throw new AggregateException("Des erreurs se sont produites lors de la validation des données.",
                innerExceptions);
        }

        Film film = new Film(titre, description, categorie, dateDeSortieInternationale, acteursParId, realisateursParId,
            duree);
        IFilm filmCree = await unitOfWork.FilmRepository.AjouterAsync(film);

        await unitOfWork.SauvegarderAsync();

        return filmCree.Id;
    }

    private static async Task<IEnumerable<Exception>> EffectuerValidations(IUnitOfWork unitOfWork, string titre,
        string description, Guid categorie, DateTime dateDeSortieInternationale, IEnumerable<Guid> acteurs,
        IEnumerable<Guid> realisateurs, ushort duree)
    {
        List<Exception> exceptions = [];

        exceptions.AddRange(await ValiderCategorieExiste(unitOfWork, categorie));
        exceptions.AddRange(await ValiderActeursExistent(unitOfWork, acteurs));
        exceptions.AddRange(await ValiderRealisateursExistent(unitOfWork, realisateurs));
        exceptions.AddRange(await ValiderFilmEstUnique(unitOfWork, titre, dateDeSortieInternationale.Year, duree));

        return exceptions;
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

    private static async Task<IEnumerable<Exception>> ValiderFilmEstUnique(IUnitOfWork unitOfWork, string titre, int
        annee, ushort duree)
    {
        List<Exception> exceptions = [];

        if (await unitOfWork.FilmRepository.ExisteAsync(f =>
                f.Titre == titre && f.DateSortieInternationale.Year == annee && f.DureeEnMinutes == duree))
        {
            exceptions.Add(new ArgumentException(
                "Un film avec le même titre, la même année de sortie et la même durée existe déjà.", nameof(titre)));
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
}