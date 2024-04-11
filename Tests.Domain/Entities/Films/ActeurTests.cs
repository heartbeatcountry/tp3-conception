using CineQuebec.Domain.Entities.Films;

using Tests.Domain.Entities.Abstract;

namespace Tests.Domain.Entities.Films;

public class ActeurTests : PersonneTests<Acteur>
{
    protected override Acteur Entite { get; set; } = null!;

    [Test]
    public void AjouterFilm_WhenGivenFilmWithEmptyGuid_ShouldThrowArgumentException()
    {
        // Act & Assert
        Assert.That(() => Entite.AjouterFilm(Guid.Empty),
            Throws.ArgumentException.With.Message.Contains("ne peut pas être vide"));
    }

    [Test]
    public void AjouterFilm_WhenGivenFilmWithValidIdNotPresentInJoueDansFilms_ShouldAddFilmToJoueDansFilms()
    {
        // Arrange
        Guid film = Guid.NewGuid();

        // Act
        Entite.AjouterFilm(film);

        // Assert
        Assert.That(Entite.JoueDansFilmsAvecId, Has.Member(film));
    }

    [Test]
    public void AjouterFilm_WhenGivenFilmWithValidIdNotPresentInJoueDansFilms_ShouldReturnTrue()
    {
        // Arrange
        Guid film = Guid.NewGuid();

        // Act
        bool result = Entite.AjouterFilm(film);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void AjouterFilm_WhenGivenFilmWithValidIdPresentInJoueDansFilms_ShouldNotAddFilmToJoueDansFilms()
    {
        // Arrange
        Guid film = Guid.NewGuid();
        Entite.AjouterFilm(film);

        // Act
        Entite.AjouterFilm(film);

        // Assert
        Assert.That(Entite.JoueDansFilmsAvecId.Count, Is.EqualTo(1));
    }

    [Test]
    public void AjouterFilm_WhenGivenFilmWithValidIdPresentInJoueDansFilms_ShouldReturnFalse()
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
    public void Constructor_Always_ShouldInitializeJoueDansFilmsToEmptyCollection()
    {
        Assert.That(Entite.JoueDansFilmsAvecId, Is.Empty);
    }

    [Test]
    public void RetirerFilm_WhenGivenFilmNotPresentInJoueDansFilms_ShouldReturnFalse()
    {
        // Arrange
        Guid film = Guid.NewGuid();

        // Act
        bool result = Entite.RetirerFilm(film);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void RetirerFilm_WhenGivenFilmPresentInJoueDansFilms_ShouldRemoveFilmFromJoueDansFilms()
    {
        // Arrange
        Guid film = Guid.NewGuid();
        Entite.AjouterFilm(film);

        // Act
        Entite.RetirerFilm(film);

        // Assert
        Assert.That(Entite.JoueDansFilmsAvecId, Has.No.Member(film));
    }

    [Test]
    public void RetirerFilm_WhenGivenFilmPresentInJoueDansFilms_ShouldReturnTrue()
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