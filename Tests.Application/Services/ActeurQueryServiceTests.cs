using CineQuebec.Application.Records.Films;
using CineQuebec.Application.Services;
using CineQuebec.Domain.Interfaces.Entities.Films;

using Moq;

namespace Tests.Application.Services;

public class ActeurQueryServiceTests : GenericServiceTests<ActeurQueryService>
{
    [Test]
    public async Task ObtenirTous_Always_ShouldReturnAllActeurs()
    {
        // Arrange
        ActeurRepositoryMock.Setup(r => r.ObtenirTousAsync(null, null))
            .ReturnsAsync(new List<IActeur>
            {
                Mock.Of<IActeur>(a => a.Id == Guid.NewGuid()), Mock.Of<IActeur>(a => a.Id == Guid.NewGuid())
            });

        // Act
        IEnumerable<ActeurDto> acteurRecords = await Service.ObtenirTous();

        // Assert
        Assert.That(acteurRecords, Has.Exactly(2).Items);
    }
}