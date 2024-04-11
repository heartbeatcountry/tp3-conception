using System.Linq.Expressions;

using CineQuebec.Application.Services;
using CineQuebec.Domain.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Films;

using Moq;

namespace Tests.Application.Services;

public class CategorieFilmCreationServiceTests : GenericServiceTests<CategorieFilmCreationService>
{
    private const string NomAffichageValide = "Thriller";

    [Test]
    public async Task CreerCategorieFilm_WhenCategorieFilmEstUnique_ShouldAddCategorieFilmToRepository()
    {
        // Arrange
        CategorieFilmRepositoryMock.Setup(r => r.ExisteAsync(It.IsAny<Expression<Func<ICategorieFilm, bool>>>()))
            .ReturnsAsync(false);
        CategorieFilmRepositoryMock.Setup(r => r.AjouterAsync(It.IsAny<CategorieFilm>()))
            .ReturnsAsync(Mock.Of<ICategorieFilm>(a => a.Id == Guid.NewGuid()));

        // Act
        await Service.CreerCategorie(NomAffichageValide);

        // Assert
        CategorieFilmRepositoryMock.Verify(r => r.AjouterAsync(It.IsAny<CategorieFilm>()));
    }

    [Test]
    public async Task CreerCategorieFilm_WhenCategorieFilmEstUnique_ShouldReturnGuidOfNewCategorieFilm()
    {
        // Arrange
        CategorieFilmRepositoryMock.Setup(r => r.ExisteAsync(It.IsAny<Expression<Func<ICategorieFilm, bool>>>()))
            .ReturnsAsync(false);
        CategorieFilmRepositoryMock.Setup(r => r.AjouterAsync(It.IsAny<CategorieFilm>()))
            .ReturnsAsync(Mock.Of<ICategorieFilm>(a => a.Id == Guid.NewGuid()));

        // Act
        Guid categorieFilmId = await Service.CreerCategorie(NomAffichageValide);

        // Assert
        Assert.That(categorieFilmId, Is.Not.EqualTo(Guid.Empty));
    }

    [Test]
    public void
        CreerCategorieFilm_WhenCategorieFilmWithSameNomAffichageAlreadyExists_ShouldThrowAggregateExceptionContainingArgumentException()
    {
        // Arrange
        CategorieFilmRepositoryMock.Setup(r => r.ExisteAsync(It.IsAny<Expression<Func<ICategorieFilm, bool>>>()))
            .ReturnsAsync(true);

        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            Service.CreerCategorie(NomAffichageValide));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("existe déjà"));
    }

    [Test]
    public async Task CreerCategorieFilm_WhenGivenNomWithLeadingOrTrailingSpaces_ShouldTrimNom()
    {
        // Arrange
        CategorieFilmRepositoryMock.Setup(r => r.ExisteAsync(It.IsAny<Expression<Func<ICategorieFilm, bool>>>()))
            .ReturnsAsync(false);
        CategorieFilmRepositoryMock.Setup(r => r.AjouterAsync(It.IsAny<CategorieFilm>()))
            .ReturnsAsync(Mock.Of<ICategorieFilm>(a => a.Id == Guid.NewGuid()));

        // Act
        await Service.CreerCategorie($" {NomAffichageValide} ");

        // Assert
        CategorieFilmRepositoryMock.Verify(r =>
            r.AjouterAsync(It.Is<CategorieFilm>(a => a.NomAffichage == NomAffichageValide)));
    }

    [Test]
    public void CreerCategorieFilm_WhenGivenNomWithOnlySpaces_ShouldThrowAggregateExceptionContainingArgumentException()
    {
        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            Service.CreerCategorie("   "));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("ne doit pas être vide"));
    }
}