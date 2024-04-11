using CineQuebec.Application.Records.Projections;
using CineQuebec.Application.Services;
using CineQuebec.Domain.Interfaces.Entities.Projections;

using Moq;

namespace Tests.Application.Services;

public class SalleQueryServiceTests : GenericServiceTests<SalleQueryService>
{
    [Test]
    public async Task ObtenirToutes_Always_ShouldReturnAllSalles()
    {
        // Arrange
        SalleRepositoryMock.Setup(r => r.ObtenirTousAsync(null, null))
            .ReturnsAsync(new List<ISalle>
            {
                Mock.Of<ISalle>(a => a.Id == Guid.NewGuid()), Mock.Of<ISalle>(a => a.Id == Guid.NewGuid())
            });

        // Act
        IEnumerable<SalleDto> salleRecords = await Service.ObtenirToutes();

        // Assert
        Assert.That(salleRecords, Has.Exactly(2).Items);
    }
}