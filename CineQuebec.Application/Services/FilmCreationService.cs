using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services;
using CineQuebec.Domain.Entities.Films;

namespace CineQuebec.Application.Services;

public class FilmCreationService(IUnitOfWorkFactory unitOfWorkFactory) : IFilmCreationService
{
	public async Task<Guid> CreerFilm(string titre, string description, Guid categorie, DateTime
		dateDeSortieInternationale, IEnumerable<Guid> acteurs, IEnumerable<Guid> realisateurs, ushort duree)
	{
		var acteursParId = acteurs as Guid[] ?? acteurs.ToArray();
		var realisateursParId = realisateurs as Guid[] ?? realisateurs.ToArray();

		using var unitOfWork = unitOfWorkFactory.Create();

		var exceptions = new List<Exception>();
		exceptions.AddRange(await ValiderActeursExistent(unitOfWork, acteursParId));
		exceptions.AddRange(await ValiderRealisateursExistent(unitOfWork, realisateursParId));

		if (await ValiderCategorieExiste(unitOfWork, categorie) is { } categorieException)
		{
			exceptions.Add(categorieException);
		}

		if (exceptions.Count != 0)
		{
			throw new AggregateException("Des erreurs se sont produites lors de la validation des données.",
				exceptions);
		}

		var film = new Film(titre, description, categorie, dateDeSortieInternationale, acteursParId, realisateursParId,
			duree);
		var filmCree = await unitOfWork.FilmRepository.AjouterAsync(film);

		await unitOfWork.SauvegarderAsync();

		return filmCree.Id;
	}

	private static async Task<IEnumerable<Exception>> ValiderActeursExistent(IUnitOfWork unitOfWork, Guid[] acteurs)
	{
		var exceptions = new List<Exception>();

		foreach (var idActeur in acteurs)
		{
			if (await unitOfWork.ActeurRepository.ObtenirParIdAsync(idActeur) is null)
			{
				exceptions.Add(new ArgumentException($"L'acteur avec l'identifiant {idActeur} n'existe pas.",
					nameof(acteurs)));
			}
		}

		return exceptions;
	}

	private static async Task<Exception?> ValiderCategorieExiste(IUnitOfWork unitOfWork, Guid categorie)
	{
		if (await unitOfWork.CategorieFilmRepository.ObtenirParIdAsync(categorie) is null)
		{
			return new ArgumentException($"La catégorie de film avec l'identifiant {categorie} n'existe pas.",
				nameof(categorie));
		}

		return null;
	}

	private static async Task<IEnumerable<Exception>> ValiderRealisateursExistent(IUnitOfWork unitOfWork,
		Guid[] realisateurs)
	{
		var exceptions = new List<Exception>();

		foreach (var idRealisateur in realisateurs)
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