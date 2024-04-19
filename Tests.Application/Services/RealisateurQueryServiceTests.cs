using CineQuebec.Application.Records.Films;
using CineQuebec.Application.Services;
using CineQuebec.Domain.Interfaces.Entities.Films;

using Moq;

namespace Tests.Application.Services;

public class RealisateurQueryServiceTests : GenericServiceTests<RealisateurQueryService>
{
    [Test]
    public async Task ObtenirTous_WhenNoRealisateurExists_ShouldReturnEmptyCollection()
    {
        // Arrange
        RealisateurRepositoryMock.Setup(r => r.ObtenirTousAsync(null, null))
            .ReturnsAsync(new List<IRealisateur>());

        // Act
        IEnumerable<RealisateurDto> realisateurRecords = (await Service.ObtenirTous()).ToArray();

        // Assert
        Assert.That(realisateurRecords, Is.Empty);
    }

    [Test]
    public async Task ObtenirTous_WhenRealisateursExist_ShouldReturnAllRealisateursOrderedByPrenomThenByNom()
    {
        // Arrange
        RealisateurRepositoryMock.Setup(r => r.ObtenirTousAsync(null, null))
            .ReturnsAsync(new List<IRealisateur>
            {
                Mock.Of<IRealisateur>(a => a.Prenom == "A" && a.Nom == "A"),
                Mock.Of<IRealisateur>(a => a.Prenom == "A" && a.Nom == "B"),
                Mock.Of<IRealisateur>(a => a.Prenom == "B" && a.Nom == "A"),
                Mock.Of<IRealisateur>(a => a.Prenom == "B" && a.Nom == "B")
            });

        // Act
        IEnumerable<RealisateurDto> realisateurRecords = (await Service.ObtenirTous()).ToArray();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(realisateurRecords, Has.Exactly(4).Items);
            Assert.That(realisateurRecords,
                Is.Ordered.By(nameof(RealisateurDto.Prenom)).Then.By(nameof(RealisateurDto.Nom)));
            Assert.That(realisateurRecords.ElementAt(0).Prenom, Is.EqualTo("A"));
            Assert.That(realisateurRecords.ElementAt(0).Nom, Is.EqualTo("A"));
            Assert.That(realisateurRecords.ElementAt(1).Prenom, Is.EqualTo("A"));
            Assert.That(realisateurRecords.ElementAt(1).Nom, Is.EqualTo("B"));
            Assert.That(realisateurRecords.ElementAt(2).Prenom, Is.EqualTo("B"));
            Assert.That(realisateurRecords.ElementAt(2).Nom, Is.EqualTo("A"));
            Assert.That(realisateurRecords.ElementAt(3).Prenom, Is.EqualTo("B"));
            Assert.That(realisateurRecords.ElementAt(3).Nom, Is.EqualTo("B"));
        });
    }
}