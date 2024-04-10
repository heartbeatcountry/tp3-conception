using System.Linq.Expressions;

using CineQuebec.Application.Records.Projections;
using CineQuebec.Application.Services;
using CineQuebec.Domain.Interfaces.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Projections;

using Moq;

namespace Tests.Application.Services;

public class ProjectionDeletionServiceTests : GenericServiceTests<ProjectionDeletionService>
{
    [Test]
    public void SupprimerProjection_WhenProjectionDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        Guid idProjection = Guid.NewGuid();
        ProjectionRepositoryMock.Setup(r => r.ObtenirParIdAsync(idProjection)).ReturnsAsync((IProjection?)null);

        // Act
        bool result = Service.SupprimerProjection(idProjection).Result;

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void SupprimerProjection_WhenProjectionExists_ShouldReturnTrue()
    {
        // Arrange
        Guid idProjection = Guid.NewGuid();
        IProjection projection = Mock.Of<IProjection>(p => p.Id == idProjection);
        ProjectionRepositoryMock.Setup(r => r.ObtenirParIdAsync(idProjection)).ReturnsAsync(projection);

        // Act
        bool result = Service.SupprimerProjection(idProjection).Result;

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void SupprimerProjection_WhenProjectionExists_ShouldDeleteProjection()
    {
        // Arrange
        Guid idProjection = Guid.NewGuid();
        IProjection projection = Mock.Of<IProjection>(p => p.Id == idProjection);
        ProjectionRepositoryMock.Setup(r => r.ObtenirParIdAsync(idProjection)).ReturnsAsync(projection);

        // Act
        Service.SupprimerProjection(idProjection).Wait();

        // Assert
        ProjectionRepositoryMock.Verify(r => r.Supprimer(projection), Times.Once);
    }
}