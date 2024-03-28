using CineQuebec.Domain.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Films;
using Moq;
using Tests.Domain.Entities.Abstract;

namespace Tests.Domain.Entities.Films;

public class RealisateurTests : PersonneTests<Realisateur>
{
	protected override Realisateur Entite { get; set; } = null!;

	[Test]
	public void AfterInstantiation_RealiseFilmsShouldBeEmpty()
	{
		Assert.That(Entite.RealiseFilms, Is.Empty);
	}

	[Test]
	public void GivenFilmToAdd_WhenAjouterFilm_ThenFilmShouldBeAddedToRealiseFilms()
	{
		// Arrange
		var film = Mock.Of<IFilm>();

		// Act
		Entite.AjouterFilm(film);

		// Assert
		Assert.That(Entite.RealiseFilms, Has.Member(film));
	}

	[Test]
	public void GivenFilmToRemove_WhenRetirerFilm_ThenFilmShouldBeRemovedFromRealiseFilms()
	{
		// Arrange
		var film = Mock.Of<IFilm>();
		Entite.AjouterFilm(film);

		// Act
		Entite.RetirerFilm(film);

		// Assert
		Assert.That(Entite.RealiseFilms, Has.No.Member(film));
	}

	[Test]
	public void GivenFilmsToAdd_WhenAjouterFilms_ThenFilmsShouldBeAddedToRealiseFilms()
	{
		// Arrange
		var films = new List<IFilm>
		{
			Mock.Of<IFilm>(m => m.Id == Guid.NewGuid()),
			Mock.Of<IFilm>(m => m.Id == Guid.NewGuid()),
		};

		// Act
		Entite.AjouterFilms(films);

		// Assert
		Assert.That(Entite.RealiseFilms, Has.Member(films[0]));
		Assert.That(Entite.RealiseFilms, Has.Member(films[1]));
	}

	[Test]
	public void GivenFilmToAdd_WhenAjouterFilm_ThenReturnTrue()
	{
		// Arrange
		var film = Mock.Of<IFilm>(m => m.Id == Guid.NewGuid());

		// Act
		var result = Entite.AjouterFilm(film);

		// Assert
		Assert.That(result, Is.True);
	}

	[Test]
	public void GivenFilmInRealiseFilms_WhenRetirerFilm_ThenReturnTrue()
	{
		// Arrange
		var film = Mock.Of<IFilm>(m => m.Id == Guid.NewGuid());
		Entite.AjouterFilm(film);

		// Act
		var result = Entite.RetirerFilm(film);

		// Assert
		Assert.That(result, Is.True);
	}

	[Test]
	public void GivenFilmNotInRealiseFilms_WhenRetirerFilm_ThenReturnFalse()
	{
		// Arrange
		var film = Mock.Of<IFilm>(m => m.Id == Guid.NewGuid());

		// Act
		var result = Entite.RetirerFilm(film);

		// Assert
		Assert.That(result, Is.False);
	}

	[Test]
	public void GivenFilmAlreadyInRealiseFilms_WhenAjouterFilm_ThenReturnFalse()
	{
		// Arrange
		var film = Mock.Of<IFilm>(m => m.Id == Guid.NewGuid());
		Entite.AjouterFilm(film);

		// Act
		var result = Entite.AjouterFilm(film);

		// Assert
		Assert.That(result, Is.False);
	}
}