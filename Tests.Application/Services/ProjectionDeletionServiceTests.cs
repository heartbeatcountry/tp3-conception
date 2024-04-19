using CineQuebec.Application.Services;
using CineQuebec.Domain.Interfaces.Entities.Projections;

using Moq;

namespace Tests.Application.Services;

public class ProjectionDeletionServiceTests : GenericServiceTests<ProjectionDeletionService>
{
    [Test]
    public async Task SupprimerProjection_WhenProjectionDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        Guid idProjection = Guid.NewGuid();
        ProjectionRepositoryMock.Setup(r => r.ObtenirParIdAsync(idProjection)).ReturnsAsync((IProjection?)null);

        // Act
        bool result = await Service.SupprimerProjection(idProjection);

        // Assert
        Assert.That(result, Is.False);
        ProjectionRepositoryMock.Verify(r => r.Supprimer(It.IsAny<IProjection>()), Times.Never);
    }

    [Test]
    public async Task SupprimerProjection_WhenProjectionExists_ShouldDeleteProjection()
    {
        // Arrange
        Guid idProjection = Guid.NewGuid();
        IProjection projection = Mock.Of<IProjection>(p => p.Id == idProjection);
        ProjectionRepositoryMock.Setup(r => r.ObtenirParIdAsync(idProjection)).ReturnsAsync(projection);

        // Act
        _ = await Service.SupprimerProjection(idProjection);

        // Assert
        ProjectionRepositoryMock.Verify(r => r.Supprimer(projection), Times.Once);
        UnitOfWorkMock.Verify(uow => uow.SauvegarderAsync(It.IsAny<CancellationToken?>()), Times.Once);
    }

    [Test]
    public async Task SupprimerProjection_WhenProjectionExists_ShouldReturnTrue()
    {
        // Arrange
        Guid idProjection = Guid.NewGuid();
        IProjection projection = Mock.Of<IProjection>(p => p.Id == idProjection);
        ProjectionRepositoryMock.Setup(r => r.ObtenirParIdAsync(idProjection)).ReturnsAsync(projection);

        // Act
        bool result = await Service.SupprimerProjection(idProjection);

        // Assert
        Assert.That(result, Is.True);
    }
}