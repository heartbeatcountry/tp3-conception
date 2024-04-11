using CineQuebec.Application.Records.Films;
using CineQuebec.Application.Services;
using CineQuebec.Domain.Interfaces.Entities.Films;

using Moq;

namespace Tests.Application.Services;

public class CategorieFilmQueryServiceTests : GenericServiceTests<CategorieFilmQueryService>
{
    [Test]
    public async Task ObtenirToutes_Always_ShouldReturnAllCategorieFilms()
    {
        // Arrange
        CategorieFilmRepositoryMock.Setup(r => r.ObtenirTousAsync(null, null))
            .ReturnsAsync(new List<ICategorieFilm>
            {
                Mock.Of<ICategorieFilm>(a => a.Id == Guid.NewGuid()),
                Mock.Of<ICategorieFilm>(a => a.Id == Guid.NewGuid())
            });

        // Act
        IEnumerable<CategorieFilmDto> categorieFilmRecords = await Service.ObtenirToutes();

        // Assert
        Assert.That(categorieFilmRecords, Has.Exactly(2).Items);
    }
}