using System.Linq.Expressions;

using CineQuebec.Application.Services;
using CineQuebec.Domain.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Films;

using Moq;

namespace Tests.Application.Services;

public class RealisateurCreationServiceTests : GenericServiceTests<RealisateurCreationService>
{
    private const string PrenomValide = "Denis";
    private const string NomValide = "Villeneuve";

    [Test]
    public async Task CreerRealisateur_WhenRealisateurEstUnique_ShouldAddRealisateurToRepository()
    {
        // Arrange
        RealisateurRepositoryMock.Setup(r => r.ExisteAsync(It.IsAny<Expression<Func<IRealisateur, bool>>>()))
            .ReturnsAsync(false);
        RealisateurRepositoryMock.Setup(r => r.AjouterAsync(It.IsAny<Realisateur>()))
            .ReturnsAsync(Mock.Of<IRealisateur>(a => a.Id == Guid.NewGuid()));

        // Act
        await Service.CreerRealisateur(PrenomValide, NomValide);

        // Assert
        RealisateurRepositoryMock.Verify(r => r.AjouterAsync(It.IsAny<Realisateur>()));
    }

    [Test]
    public async Task CreerRealisateur_WhenRealisateurEstUnique_ShouldReturnGuidOfNewRealisateur()
    {
        // Arrange
        RealisateurRepositoryMock.Setup(r => r.ExisteAsync(It.IsAny<Expression<Func<IRealisateur, bool>>>()))
            .ReturnsAsync(false);
        RealisateurRepositoryMock.Setup(r => r.AjouterAsync(It.IsAny<Realisateur>()))
            .ReturnsAsync(Mock.Of<IRealisateur>(a => a.Id == Guid.NewGuid()));

        // Act
        Guid realisateurId = await Service.CreerRealisateur(PrenomValide, NomValide);

        // Assert
        Assert.That(realisateurId, Is.Not.EqualTo(Guid.Empty));
    }

    [Test]
    public void
        CreerRealisateur_WhenRealisateurWithSameNomAndPrenomAlreadyExists_ShouldThrowAggregateExceptionContainingArgumentException()
    {
        // Arrange
        RealisateurRepositoryMock.Setup(r => r.ExisteAsync(It.IsAny<Expression<Func<IRealisateur, bool>>>()))
            .ReturnsAsync(true);

        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            Service.CreerRealisateur(PrenomValide, NomValide));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("existe déjà"));
    }

    [Test]
    public async Task CreerRealisateur_WhenGivenNomWithLeadingOrTrailingSpaces_ShouldTrimNom()
    {
        // Arrange
        RealisateurRepositoryMock.Setup(r => r.ExisteAsync(It.IsAny<Expression<Func<IRealisateur, bool>>>()))
            .ReturnsAsync(false);
        RealisateurRepositoryMock.Setup(r => r.AjouterAsync(It.IsAny<Realisateur>()))
            .ReturnsAsync(Mock.Of<IRealisateur>(a => a.Id == Guid.NewGuid()));

        // Act
        await Service.CreerRealisateur(PrenomValide, $" {NomValide} ");

        // Assert
        RealisateurRepositoryMock.Verify(r => r.AjouterAsync(It.Is<Realisateur>(a => a.Nom == NomValide)));
    }

    [Test]
    public void CreerRealisateur_WhenGivenNomWithOnlySpaces_ShouldThrowAggregateExceptionContainingArgumentException()
    {
        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            Service.CreerRealisateur(PrenomValide, "   "));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("ne doit pas être vide"));
    }

    [Test]
    public async Task CreerRealisateur_WhenGivenPrenomWithLeadingOrTrailingSpaces_ShouldTrimPrenom()
    {
        // Arrange
        RealisateurRepositoryMock.Setup(r => r.ExisteAsync(It.IsAny<Expression<Func<IRealisateur, bool>>>()))
            .ReturnsAsync(false);
        RealisateurRepositoryMock.Setup(r => r.AjouterAsync(It.IsAny<Realisateur>()))
            .ReturnsAsync(Mock.Of<IRealisateur>(a => a.Id == Guid.NewGuid()));

        // Act
        await Service.CreerRealisateur($" {PrenomValide} ", NomValide);

        // Assert
        RealisateurRepositoryMock.Verify(r => r.AjouterAsync(It.Is<Realisateur>(a => a.Prenom == PrenomValide)));
    }

    [Test]
    public void CreerRealisateur_WhenGivenPrenomWithOnlySpaces_ShouldThrowAggregateExceptionContainingArgumentException()
    {
        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            Service.CreerRealisateur("   ", NomValide));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("ne doit pas être vide"));
    }
}