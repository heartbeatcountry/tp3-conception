using CineQuebec.Domain.Entities.Abstract;
using CineQuebec.Domain.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Films;
using Moq;

namespace Tests.Domain.Entities.Films;

public class ActeurTests
{
	private const string PrenomValide = "Michel";
	private const string NomValide = "Sardou";
	private Acteur _acteur;

	[SetUp]
	public void Setup()
	{
		_acteur = new Acteur(PrenomValide, NomValide);
	}

	[Test]
	public void OnInstantiation_ConstructorShouldThrowArgumentExceptionWhenPrenomIsNullOrWhitespace()
	{
		Assert.That(() => new Acteur(null, NomValide), Throws.ArgumentException);
		Assert.That(() => new Acteur(string.Empty, NomValide), Throws.ArgumentException);
		Assert.That(() => new Acteur(" ", NomValide), Throws.ArgumentException);
	}

	[Test]
	public void OnInstantiation_ConstructorShouldThrowArgumentExceptionWhenNomIsNullOrWhitespace()
	{
		Assert.That(() => new Acteur(PrenomValide, null), Throws.ArgumentException);
		Assert.That(() => new Acteur(PrenomValide, string.Empty), Throws.ArgumentException);
		Assert.That(() => new Acteur(PrenomValide, " "), Throws.ArgumentException);
	}

	[Test]
	public void AfterInstantiation_ActeurShouldBeOfTypePersonne()
	{
		Assert.That(_acteur, Is.InstanceOf<Personne>());
	}

	[Test]
	public void AfterInstantiation_PrenomShouldBeEqualToPrenomGivenInConstructor()
	{
		Assert.That(_acteur.Prenom, Is.EqualTo(PrenomValide));
	}

	[Test]
	public void AfterInstantiation_NomShouldBeEqualToNomGivenInConstructor()
	{
		Assert.That(_acteur.Nom, Is.EqualTo(NomValide));
	}

	[Test]
	public void AfterInstantiation_NomCompletShouldBeEqualToPrenomAndNomGivenInConstructor()
	{
		Assert.That(_acteur.NomComplet, Is.EqualTo($"{PrenomValide} {NomValide}"));
	}

	[Test]
	public void AfterInstantiation_IdShouldBeEmpty()
	{
		Assert.That(_acteur.Id, Is.EqualTo(Guid.Empty));
	}

	[Test]
	public void AfterInstantiation_JoueDansFilmsShouldBeEmpty()
	{
		Assert.That(_acteur.JoueDansFilms, Is.Empty);
	}

	[Test]
	public void GivenFilmToAdd_WhenAjouterFilm_ThenFilmShouldBeAddedToJoueDansFilms()
	{
		// Arrange
		var film = Mock.Of<IFilm>(m => m.Id == Guid.NewGuid());

		// Act
		_acteur.AjouterFilm(film);

		// Assert
		Assert.That(_acteur.JoueDansFilms, Has.Member(film));
	}

	[Test]
	public void GivenFilmToRemove_WhenRetirerFilm_ThenFilmShouldBeRemovedFromJoueDansFilms()
	{
		// Arrange
		var film = Mock.Of<IFilm>(m => m.Id == Guid.NewGuid());
		_acteur.AjouterFilm(film);

		// Act
		_acteur.RetirerFilm(film);

		// Assert
		Assert.That(_acteur.JoueDansFilms, Has.No.Member(film));
	}

	[Test]
	public void GivenFilmsToAdd_WhenAjouterFilms_ThenFilmsShouldBeAddedToJoueDansFilms()
	{
		// Arrange
		var films = new List<IFilm>
		{
			Mock.Of<IFilm>(m => m.Id == Guid.NewGuid()),
			Mock.Of<IFilm>(m => m.Id == Guid.NewGuid()),
		};

		// Act
		_acteur.AjouterFilms(films);

		// Assert
		Assert.That(_acteur.JoueDansFilms, Has.Member(films[0]));
		Assert.That(_acteur.JoueDansFilms, Has.Member(films[1]));
	}

	[Test]
	public void GivenFilmToAdd_WhenAjouterFilm_ThenReturnTrue()
	{
		// Arrange
		var film = Mock.Of<IFilm>(m => m.Id == Guid.NewGuid());

		// Act
		var result = _acteur.AjouterFilm(film);

		// Assert
		Assert.That(result, Is.True);
	}

	[Test]
	public void GivenFilmToRemove_WhenRetirerFilm_ThenReturnTrue()
	{
		// Arrange
		var film = Mock.Of<IFilm>(m => m.Id == Guid.NewGuid());
		_acteur.AjouterFilm(film);

		// Act
		var result = _acteur.RetirerFilm(film);

		// Assert
		Assert.That(result, Is.True);
	}

	[Test]
	public void GivenFilmNotInJoueDansFilms_WhenRetirerFilm_ThenReturnFalse()
	{
		// Arrange
		var film = Mock.Of<IFilm>(m => m.Id == Guid.NewGuid());

		// Act
		var result = _acteur.RetirerFilm(film);

		// Assert
		Assert.That(result, Is.False);
	}

	[Test]
	public void GivenFilmAlreadyInJoueDansFilms_WhenAjouterFilm_ThenReturnFalse()
	{
		// Arrange
		var film = Mock.Of<IFilm>(m => m.Id == Guid.NewGuid());
		_acteur.AjouterFilm(film);

		// Act
		var result = _acteur.AjouterFilm(film);

		// Assert
		Assert.That(result, Is.False);
	}
}