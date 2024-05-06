using CineQuebec.Application.Records.Films;
using CineQuebec.Application.Services.Films;
using CineQuebec.Domain.Interfaces.Entities.Films;

using Moq;

namespace Tests.Application.Services.Films;

public class CategorieFilmQueryServiceTests : GenericServiceTests<CategorieFilmQueryService>
{
    [Test]
    public async Task ObtenirToutes_WhenNoCategorieFilmsExist_ShouldReturnEmptyList()
    {
        // Arrange
        CategorieFilmRepositoryMock.Setup(r => r.ObtenirTousAsync(null, null))
            .ReturnsAsync(new List<ICategorieFilm>());

        // Act
        IEnumerable<CategorieFilmDto> categorieFilmRecords = await Service.ObtenirToutes();

        // Assert
        Assert.That(categorieFilmRecords, Is.Empty);
    }

    [Test]
    public async Task ObtenirToutes_WhenSomeCategorieFilmsExist_ShouldReturnCategorieFilmsOrderedByNomAffichage()
    {
        // Arrange
        CategorieFilmRepositoryMock.Setup(r => r.ObtenirTousAsync(null, null))
            .ReturnsAsync(new List<ICategorieFilm>
            {
                Mock.Of<ICategorieFilm>(a => a.NomAffichage == "Thriller"),
                Mock.Of<ICategorieFilm>(a => a.NomAffichage == "Action")
            });

        // Act
        IEnumerable<CategorieFilmDto> categorieFilmRecords = (await Service.ObtenirToutes()).ToArray();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(categorieFilmRecords, Has.Exactly(2).Items);
            Assert.That(categorieFilmRecords, Is.Ordered.By(nameof(CategorieFilmDto.NomAffichage)));
            Assert.That(categorieFilmRecords.First().NomAffichage, Is.EqualTo("Action"));
            Assert.That(categorieFilmRecords.Last().NomAffichage, Is.EqualTo("Thriller"));
        });
    }
}