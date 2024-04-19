using CineQuebec.Application.Records.Films;
using CineQuebec.Application.Services;
using CineQuebec.Domain.Interfaces.Entities.Films;

using Moq;

namespace Tests.Application.Services;

public class ActeurQueryServiceTests : GenericServiceTests<ActeurQueryService>
{
    [Test]
    public async Task ObtenirTous_WhenActeursExist_ShouldReturnAllActeursOrderedByPrenomThenByNom()
    {
        // Arrange
        ActeurRepositoryMock.Setup(r => r.ObtenirTousAsync(null, null))
            .ReturnsAsync(new List<IActeur>
            {
                Mock.Of<IActeur>(a => a.Prenom == "A" && a.Nom == "A"),
                Mock.Of<IActeur>(a => a.Prenom == "A" && a.Nom == "B"),
                Mock.Of<IActeur>(a => a.Prenom == "B" && a.Nom == "A"),
                Mock.Of<IActeur>(a => a.Prenom == "B" && a.Nom == "B")
            });

        // Act
        IEnumerable<ActeurDto> acteurRecords = (await Service.ObtenirTous()).ToArray();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(acteurRecords, Has.Exactly(4).Items);
            Assert.That(acteurRecords, Is.Ordered.By(nameof(ActeurDto.Prenom)).Then.By(nameof(ActeurDto.Nom)));
            Assert.That(acteurRecords.ElementAt(0).Prenom, Is.EqualTo("A"));
            Assert.That(acteurRecords.ElementAt(0).Nom, Is.EqualTo("A"));
            Assert.That(acteurRecords.ElementAt(1).Prenom, Is.EqualTo("A"));
            Assert.That(acteurRecords.ElementAt(1).Nom, Is.EqualTo("B"));
            Assert.That(acteurRecords.ElementAt(2).Prenom, Is.EqualTo("B"));
            Assert.That(acteurRecords.ElementAt(2).Nom, Is.EqualTo("A"));
            Assert.That(acteurRecords.ElementAt(3).Prenom, Is.EqualTo("B"));
            Assert.That(acteurRecords.ElementAt(3).Nom, Is.EqualTo("B"));
        });
    }

    [Test]
    public async Task ObtenirTous_WhenNoActeurExists_ShouldReturnEmptyList()
    {
        // Arrange
        ActeurRepositoryMock.Setup(r => r.ObtenirTousAsync(null, null))
            .ReturnsAsync(new List<IActeur>());

        // Act
        IEnumerable<ActeurDto> acteurRecords = await Service.ObtenirTous();

        // Assert
        Assert.That(acteurRecords, Is.Empty);
    }
}