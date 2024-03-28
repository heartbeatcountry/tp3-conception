using CineQuebec.Domain.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Films;
using Moq;
using Tests.Domain.Entities.Abstract;

namespace Tests.Domain.Entities.Films;

public class ActeurTests : PersonneTests<Acteur>
{
	protected override Acteur Entite { get; set; } = null!;

	[Test]
	public void Constructor_Always_ShouldInitializeJoueDansFilmsToEmptyCollection()
	{
		Assert.That(Entite.JoueDansFilms, Is.Empty);
	}

	[Test]
	public void AjouterFilm_WhenGivenFilmWithUniqueId_ShouldAddFilmToJoueDansFilms()
	{
		// Arrange
		var film = Mock.Of<IFilm>(m => m.Id == Guid.NewGuid());

		// Act
		Entite.AjouterFilm(film);

		// Assert
		Assert.That(Entite.JoueDansFilms, Has.Member(film));
	}

	[Test]
	public void AjouterFilm_WhenGivenFilmWithNonUniqueId_ShouldThrowArgumentException()
	{
		// Arrange
		var film = Mock.Of<IFilm>(m => m.Id == Guid.Empty);

		// Act
		void AjouterFilm() => Entite.AjouterFilm(film);

		// Assert
		Assert.That(AjouterFilm, Throws.ArgumentException);
	}

	[Test]
	public void RetirerFilm_WhenGivenFilmPresentInJoueDansFilms_ShouldRemoveFilmFromJoueDansFilms()
	{
		// Arrange
		var film = Mock.Of<IFilm>(f => f.Id == Guid.NewGuid());
		Entite.AjouterFilm(film);

		// Act
		Entite.RetirerFilm(film);

		// Assert
		Assert.That(Entite.JoueDansFilms, Has.No.Member(film));
	}

	[Test]
	public void AjouterFilms_WhenGivenFilmsWithDistinctIds_ShouldAddFilmsToJoueDansFilms()
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
		Assert.That(Entite.JoueDansFilms, Has.Member(films[0]));
		Assert.That(Entite.JoueDansFilms, Has.Member(films[1]));
	}

	[Test]
	public void AjouterFilm_WhenGivenFilmNotPresentInJoueDansFilms_ShouldReturnTrue()
	{
		// Arrange
		var film = Mock.Of<IFilm>(m => m.Id == Guid.NewGuid());

		// Act
		var result = Entite.AjouterFilm(film);

		// Assert
		Assert.That(result, Is.True);
	}

	[Test]
	public void RetirerFilm_WhenGivenFilmPresentInJoueDansFilms_ShouldReturnTrue()
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
	public void RetirerFilm_WhenGivenFilmNotPresentInJoueDansFilms_ShouldReturnFalse()
	{
		// Arrange
		var film = Mock.Of<IFilm>(m => m.Id == Guid.NewGuid());

		// Act
		var result = Entite.RetirerFilm(film);

		// Assert
		Assert.That(result, Is.False);
	}

	[Test]
	public void AjouterFilm_WhenGivenFilmPresentInJoueDansFilms_ShouldReturnFalse()
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