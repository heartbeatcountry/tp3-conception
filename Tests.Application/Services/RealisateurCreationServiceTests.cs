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
    public async Task CreerRealisateur_WhenGivenNomWithLeadingOrTrailingSpaces_ShouldTrimNom()
    {
        // Arrange
        RealisateurRepositoryMock.Setup(r => r.ExisteAsync(It.IsAny<Expression<Func<IRealisateur, bool>>>()))
            .ReturnsAsync(false);
        RealisateurRepositoryMock.Setup(r => r.AjouterAsync(It.IsAny<Realisateur>()))
            .ReturnsAsync(Mock.Of<IRealisateur>(a => a.Id == Guid.NewGuid()));

        // Act
        _ = await Service.CreerRealisateur(PrenomValide, $" {NomValide} ");

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
        _ = await Service.CreerRealisateur($" {PrenomValide} ", NomValide);

        // Assert
        RealisateurRepositoryMock.Verify(r => r.AjouterAsync(It.Is<Realisateur>(a => a.Prenom == PrenomValide)));
    }

    [Test]
    public void
        CreerRealisateur_WhenGivenPrenomWithOnlySpaces_ShouldThrowAggregateExceptionContainingArgumentException()
    {
        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            Service.CreerRealisateur("   ", NomValide));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("ne doit pas être vide"));
    }

    [Test]
    public async Task CreerRealisateur_WhenRealisateurEstUnique_ShouldCreateAndReturnNewRealisateur()
    {
        // Arrange
        IRealisateur mockRealisateur = Mock.Of<IRealisateur>(a => a.Id == Guid.NewGuid());
        RealisateurRepositoryMock.Setup(r => r.ExisteAsync(It.IsAny<Expression<Func<IRealisateur, bool>>>()))
            .ReturnsAsync(false);
        RealisateurRepositoryMock.Setup(r => r.AjouterAsync(It.IsAny<Realisateur>()))
            .ReturnsAsync(mockRealisateur);

        // Act
        Guid realisateurId = await Service.CreerRealisateur(PrenomValide, NomValide);

        // Assert
        Assert.That(realisateurId, Is.EqualTo(mockRealisateur.Id));
    }

    [Test]
    public async Task CreerRealisateur_WhenRealisateurEstUnique_ShouldPersistNewRealisateur()
    {
        // Arrange
        RealisateurRepositoryMock.Setup(r => r.ExisteAsync(It.IsAny<Expression<Func<IRealisateur, bool>>>()))
            .ReturnsAsync(false);
        RealisateurRepositoryMock.Setup(r => r.AjouterAsync(It.IsAny<Realisateur>()))
            .ReturnsAsync(Mock.Of<IRealisateur>(a => a.Id == Guid.NewGuid()));

        // Act
        await Service.CreerRealisateur(PrenomValide, NomValide);

        // Assert
        RealisateurRepositoryMock.Verify(r => r.AjouterAsync(It.IsAny<Realisateur>()), Times.Once);
        UnitOfWorkMock.Verify(u => u.SauvegarderAsync(It.IsAny<CancellationToken?>()), Times.Once);
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
}