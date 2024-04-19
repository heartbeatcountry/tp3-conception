using CineQuebec.Application.Services;
using CineQuebec.Domain.Interfaces.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Projections;

using Moq;

namespace Tests.Application.Services;

public class FilmDeletionServiceTests : GenericServiceTests<FilmDeletionService>
{
    [Test]
    public async Task SupprimerFilm_WhenFilmDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        Guid idFilm = Guid.NewGuid();
        FilmRepositoryMock.Setup(r => r.ObtenirParIdAsync(idFilm)).ReturnsAsync((IFilm?)null);

        // Act
        bool result = await Service.SupprimerFilm(idFilm);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task SupprimerFilm_WhenFilmExists_ShouldDeleteFilm()
    {
        // Arrange
        Guid idFilm = Guid.NewGuid();
        IFilm film = Mock.Of<IFilm>(p => p.Id == idFilm);
        FilmRepositoryMock.Setup(r => r.ObtenirParIdAsync(idFilm)).ReturnsAsync(film);

        // Act
        _ = await Service.SupprimerFilm(idFilm);

        // Assert
        FilmRepositoryMock.Verify(r => r.Supprimer(film), Times.Once);
    }

    [Test]
    public async Task SupprimerFilm_WhenFilmExists_ShouldReturnTrue()
    {
        // Arrange
        Guid idFilm = Guid.NewGuid();
        IFilm film = Mock.Of<IFilm>(p => p.Id == idFilm);
        FilmRepositoryMock.Setup(r => r.ObtenirParIdAsync(idFilm)).ReturnsAsync(film);

        // Act
        bool result = await Service.SupprimerFilm(idFilm);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public async Task SupprimerFilm_WhenFilmExistsAndHasMultipleProjections_ShouldDeleteAllProjections()
    {
        // Arrange
        Guid idFilm = Guid.NewGuid();
        IFilm film = Mock.Of<IFilm>(p => p.Id == idFilm);
        FilmRepositoryMock.Setup(r => r.ObtenirParIdAsync(idFilm)).ReturnsAsync(film);
        ProjectionRepositoryMock.Setup(r => r.ObtenirTousAsync(pr => pr.IdFilm == idFilm, null))
            .ReturnsAsync(new[] { Mock.Of<IProjection>(), Mock.Of<IProjection>(), Mock.Of<IProjection>() });

        // Act
        _ = await Service.SupprimerFilm(idFilm);

        // Assert
        ProjectionRepositoryMock.Verify(r => r.Supprimer(It.IsAny<IProjection>()), Times.Exactly(3));
    }

    [Test]
    public async Task SupprimerFilm_WhenFilmExistsAndHasNoProjections_ShouldNotAttemptToDeleteProjections()
    {
        // Arrange
        Guid idFilm = Guid.NewGuid();
        IFilm film = Mock.Of<IFilm>(p => p.Id == idFilm);
        FilmRepositoryMock.Setup(r => r.ObtenirParIdAsync(idFilm)).ReturnsAsync(film);
        ProjectionRepositoryMock.Setup(r => r.ObtenirTousAsync(pr => pr.IdFilm == idFilm, null))
            .ReturnsAsync(Array.Empty<IProjection>());

        // Act
        _ = await Service.SupprimerFilm(idFilm);

        // Assert
        ProjectionRepositoryMock.Verify(r => r.Supprimer(It.IsAny<IProjection>()), Times.Never);
    }
}