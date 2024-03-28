using CineQuebec.Domain.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Films;
using Moq;
using Tests.Domain.Entities.Abstract;

namespace Tests.Domain.Entities.Films;

public class RealisateurTests : PersonneTests<Realisateur>
{
	protected override Realisateur Entite { get; set; } = null!;

	[Test]
	public void Constructor_Always_ShouldInitializeRealiseFilmsToEmptyCollection()
	{
		Assert.That(Entite.RealiseFilms, Is.Empty);
	}

	[Test]
	public void AjouterFilm_WhenGivenFilmWithValidIdNotPresentInRealiseFilms_ShouldAddFilmToRealiseFilms()
	{
		// Arrange
		var film = Mock.Of<IFilm>();

		// Act
		Entite.AjouterFilm(film);

		// Assert
		Assert.That(Entite.RealiseFilms, Has.Member(film));
	}

	[Test]
	public void RetirerFilm_WhenGivenFilmWithValidIdPresentInRealiseFilms_ShouldRemoveFilmFromRealiseFilms()
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
	public void AjouterFilms_WhenGivenFilmsWithValidIdNotPresentInRealiseFilms_ShouldAddFilmsToRealiseFilms()
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
	public void AjouterFilm_WhenGivenFilmWithValidIdNotPresentInRealiseFilms_ShouldReturnTrue()
	{
		// Arrange
		var film = Mock.Of<IFilm>(m => m.Id == Guid.NewGuid());

		// Act
		var result = Entite.AjouterFilm(film);

		// Assert
		Assert.That(result, Is.True);
	}

	[Test]
	public void RetirerFilm_WhenGivenFilmWithValidIdPresentInRealiseFilms_ShouldReturnTrue()
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
	public void RetirerFilm_WhenGivenFilmNotPresentInRealiseFilms_ShouldReturnFalse()
	{
		// Arrange
		var film = Mock.Of<IFilm>(m => m.Id == Guid.NewGuid());

		// Act
		var result = Entite.RetirerFilm(film);

		// Assert
		Assert.That(result, Is.False);
	}

	[Test]
	public void AjouterFilm_WhenGivenFilmAlreadyInRealiseFilms_ShouldReturnFalse()
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