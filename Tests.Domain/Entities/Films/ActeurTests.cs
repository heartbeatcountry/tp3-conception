using CineQuebec.Domain.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Films;
using Moq;
using Tests.Domain.Entities.Abstract;

namespace Tests.Domain.Entities.Films;

public class ActeurTests : PersonneTests<Acteur>
{
	protected override Acteur Entite { get; set; } = null!;

	[Test]
	public void AjouterFilm_WhenGivenFilmWithEmptyGuid_ShouldThrowArgumentException()
	{
		// Arrange
		var film = Mock.Of<IFilm>(m => m.Id == Guid.Empty);

		// Act & Assert
		Assert.That(() => Entite.AjouterFilm(film),
			Throws.ArgumentException.With.Message.Contains("Le film doit avoir un identifiant unique."));
	}

	[Test]
	public void AjouterFilm_WhenGivenFilmWithValidIdNotPresentInJoueDansFilms_ShouldAddFilmToJoueDansFilms()
	{
		// Arrange
		var film = Mock.Of<IFilm>(m => m.Id == Guid.NewGuid());

		// Act
		Entite.AjouterFilm(film);

		// Assert
		Assert.That(Entite.JoueDansFilms, Has.Member(film));
	}

	[Test]
	public void AjouterFilm_WhenGivenFilmWithValidIdNotPresentInJoueDansFilms_ShouldReturnTrue()
	{
		// Arrange
		var film = Mock.Of<IFilm>(m => m.Id == Guid.NewGuid());

		// Act
		var result = Entite.AjouterFilm(film);

		// Assert
		Assert.That(result, Is.True);
	}

	[Test]
	public void AjouterFilm_WhenGivenFilmWithValidIdPresentInJoueDansFilms_ShouldNotAddFilmToJoueDansFilms()
	{
		// Arrange
		var film = Mock.Of<IFilm>(m => m.Id == Guid.NewGuid());
		Entite.AjouterFilm(film);

		// Act
		Entite.AjouterFilm(film);

		// Assert
		Assert.That(Entite.JoueDansFilms.Count, Is.EqualTo(1));
	}

	[Test]
	public void AjouterFilm_WhenGivenFilmWithValidIdPresentInJoueDansFilms_ShouldReturnFalse()
	{
		// Arrange
		var film = Mock.Of<IFilm>(m => m.Id == Guid.NewGuid());
		Entite.AjouterFilm(film);

		// Act
		var result = Entite.AjouterFilm(film);

		// Assert
		Assert.That(result, Is.False);
	}

	[Test]
	public void Constructor_Always_ShouldInitializeJoueDansFilmsToEmptyCollection()
	{
		Assert.That(Entite.JoueDansFilms, Is.Empty);
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
}