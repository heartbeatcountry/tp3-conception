using System.Linq.Expressions;

using CineQuebec.Application.Services.Films;
using CineQuebec.Domain.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Films;

using Moq;

namespace Tests.Application.Services.Films;

public class CategorieFilmCreationServiceTests : GenericServiceTests<CategorieFilmCreationService>
{
    private const string NomAffichageValide = "Thriller";

    [Test]
    public async Task CreerCategorieFilm_WhenCategorieFilmIsUniqueAndValid_ShouldCreateAndReturnNewCategorieFilm()
    {
        // Arrange
        ICategorieFilm mockCategorieFilm = Mock.Of<ICategorieFilm>(a => a.Id == Guid.NewGuid());
        CategorieFilmRepositoryMock.Setup(r => r.ExisteAsync(It.IsAny<Expression<Func<ICategorieFilm, bool>>>()))
            .ReturnsAsync(false);
        CategorieFilmRepositoryMock.Setup(r => r.AjouterAsync(It.IsAny<CategorieFilm>()))
            .ReturnsAsync(mockCategorieFilm);

        // Act
        Guid categorieFilm = await Service.CreerCategorie(NomAffichageValide);

        // Assert
        Assert.That(categorieFilm, Is.EqualTo(mockCategorieFilm.Id));
    }

    [Test]
    public async Task CreerCategorieFilm_WhenCategorieFilmIsUniqueAndValid_ShouldPersistNewCategorieFilm()
    {
        // Arrange
        CategorieFilmRepositoryMock.Setup(r => r.ExisteAsync(It.IsAny<Expression<Func<ICategorieFilm, bool>>>()))
            .ReturnsAsync(false);
        CategorieFilmRepositoryMock.Setup(r => r.AjouterAsync(It.IsAny<CategorieFilm>()))
            .ReturnsAsync(Mock.Of<ICategorieFilm>(a => a.Id == Guid.NewGuid()));

        // Act
        _ = await Service.CreerCategorie(NomAffichageValide);

        // Assert
        CategorieFilmRepositoryMock.Verify(r => r.AjouterAsync(It.IsAny<CategorieFilm>()), Times.Once);
        UnitOfWorkMock.Verify(u => u.SauvegarderAsync(It.IsAny<CancellationToken?>()), Times.Once);
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
        _ = await Service.CreerCategorie($" {NomAffichageValide} ");

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

    [Test]
    public void
        CreerCategorieFilm_WhenOtherCategorieFilmWithSameNomAffichageAlreadyExists_ShouldThrowAggregateExceptionContainingArgumentException()
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
}