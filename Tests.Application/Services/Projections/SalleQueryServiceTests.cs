using CineQuebec.Application.Records.Projections;
using CineQuebec.Application.Services.Projections;
using CineQuebec.Domain.Interfaces.Entities.Projections;

using Moq;

namespace Tests.Application.Services.Projections;

public class SalleQueryServiceTests : GenericServiceTests<SalleQueryService>
{
    [Test]
    public async Task ObtenirToutes_WhenNoSalleExists_ShouldReturnEmptyCollection()
    {
        // Arrange
        SalleRepositoryMock.Setup(r => r.ObtenirTousAsync(null, null))
            .ReturnsAsync(new List<ISalle>());

        // Act
        IEnumerable<SalleDto> salleRecords = (await Service.ObtenirToutes()).ToArray();

        // Assert
        Assert.That(salleRecords, Is.Empty);
    }

    [Test]
    public async Task ObtenirToutes_WhenSallesExist_ShouldReturnAllSallesOrderedByNumero()
    {
        // Arrange
        SalleRepositoryMock.Setup(r => r.ObtenirTousAsync(null, null))
            .ReturnsAsync(new List<ISalle>
            {
                Mock.Of<ISalle>(a => a.Numero == 1),
                Mock.Of<ISalle>(a => a.Numero == 2),
                Mock.Of<ISalle>(a => a.Numero == 3),
                Mock.Of<ISalle>(a => a.Numero == 4)
            });

        // Act
        IEnumerable<SalleDto> salleRecords = (await Service.ObtenirToutes()).ToArray();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(salleRecords, Has.Exactly(4).Items);
            Assert.That(salleRecords, Is.Ordered.By(nameof(SalleDto.Numero)));
            Assert.That(salleRecords.ElementAt(0).Numero, Is.EqualTo(1));
            Assert.That(salleRecords.ElementAt(1).Numero, Is.EqualTo(2));
            Assert.That(salleRecords.ElementAt(2).Numero, Is.EqualTo(3));
            Assert.That(salleRecords.ElementAt(3).Numero, Is.EqualTo(4));
        });
    }
}