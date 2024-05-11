using CineQuebec.Domain.Entities.Films;
using CineQuebec.Domain.Exceptions.Entities.Films;

using Tests.Domain.Entities.Abstract;

namespace Tests.Domain.Entities.Films;

public class RealisateurTests : PersonneTests<Realisateur>
{
    protected override Realisateur Entite { get; set; } = null!;

    [Test]
    public void AjouterFilm_WhenGivenFilmWithEmptyGuid_ShouldThrowArgumentException()
    {
        // Act & Assert
        Assert.That(() => Entite.AjouterFilm(Guid.Empty), Throws.InstanceOf<FilmGuidNullException>());
    }

    [Test]
    public void AjouterFilm_WhenGivenFilmWithValidIdAlreadyPresentInRealiseFilms_ShouldNotAddFilmToRealiseFilms()
    {
        // Arrange
        Guid film = Guid.NewGuid();
        Entite.AjouterFilm(film);

        // Act
        Entite.AjouterFilm(film);

        // Assert
        Assert.That(Entite.RealiseFilmsAvecId.Count, Is.EqualTo(1));
    }

    [Test]
    public void AjouterFilm_WhenGivenFilmWithValidIdAlreadyPresentInRealiseFilms_ShouldReturnFalse()
    {
        // Arrange
        Guid film = Guid.NewGuid();
        Entite.AjouterFilm(film);

        // Act
        bool result = Entite.AjouterFilm(film);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void AjouterFilm_WhenGivenFilmWithValidIdNotPresentInRealiseFilms_ShouldAddFilmToRealiseFilms()
    {
        // Arrange
        Guid film = Guid.NewGuid();

        // Act
        Entite.AjouterFilm(film);

        // Assert
        Assert.That(Entite.RealiseFilmsAvecId, Has.Member(film));
    }

    [Test]
    public void AjouterFilm_WhenGivenFilmWithValidIdNotPresentInRealiseFilms_ShouldReturnTrue()
    {
        // Arrange
        Guid film = Guid.NewGuid();

        // Act
        bool result = Entite.AjouterFilm(film);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void Constructor_Always_ShouldInitializeRealiseFilmsToEmptyCollection()
    {
        Assert.That(Entite.RealiseFilmsAvecId, Is.Empty);
    }

    [Test]
    public void RetirerFilm_WhenGivenFilmNotPresentInRealiseFilms_ShouldReturnFalse()
    {
        // Arrange
        Guid film = Guid.NewGuid();

        // Act
        bool result = Entite.RetirerFilm(film);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void RetirerFilm_WhenGivenFilmWithValidIdPresentInRealiseFilms_ShouldRemoveFilmFromRealiseFilms()
    {
        // Arrange
        Guid film = Guid.NewGuid();
        Entite.AjouterFilm(film);

        // Act
        Entite.RetirerFilm(film);

        // Assert
        Assert.That(Entite.RealiseFilmsAvecId, Has.No.Member(film));
    }

    [Test]
    public void RetirerFilm_WhenGivenFilmWithValidIdPresentInRealiseFilms_ShouldReturnTrue()
    {
        // Arrange
        Guid film = Guid.NewGuid();
        Entite.AjouterFilm(film);

        // Act
        bool result = Entite.RetirerFilm(film);

        // Assert
        Assert.That(result, Is.True);
    }
}