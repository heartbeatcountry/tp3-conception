using CineQuebec.Application.Records.Films;
using CineQuebec.Application.Services;
using CineQuebec.Domain.Interfaces.Entities.Films;

using Moq;

namespace Tests.Application.Services;

public class RealisateurQueryServiceTests : GenericServiceTests<RealisateurQueryService>
{
    [Test]
    public async Task ObtenirTous_Always_ShouldReturnAllRealisateurs()
    {
        // Arrange
        RealisateurRepositoryMock.Setup(r => r.ObtenirTousAsync(null, null))
            .ReturnsAsync(new List<IRealisateur>
            {
                Mock.Of<IRealisateur>(a => a.Id == Guid.NewGuid()),
                Mock.Of<IRealisateur>(a => a.Id == Guid.NewGuid())
            });

        // Act
        IEnumerable<RealisateurDto> realisateurRecords = await Service.ObtenirTous();

        // Assert
        Assert.That(realisateurRecords, Has.Exactly(2).Items);
    }
}