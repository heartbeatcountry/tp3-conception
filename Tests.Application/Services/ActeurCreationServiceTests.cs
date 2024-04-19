using System.Linq.Expressions;

using CineQuebec.Application.Services;
using CineQuebec.Domain.Interfaces.Entities.Films;

using Moq;

namespace Tests.Application.Services;

public class ActeurCreationServiceTests : GenericServiceTests<ActeurCreationService>
{
    private const string PrenomValide = "Jean";
    private const string NomValide = "Tremblay";

    [Test]
    public async Task CreerActeur_WhenActeurIsUniqueAndValid_ShouldCreateAndReturnNewActeur()
    {
        // Arrange
        IActeur mockActeur = Mock.Of<IActeur>(a => a.Id == Guid.NewGuid());
        ActeurRepositoryMock.Setup(r => r.ExisteAsync(It.IsAny<Expression<Func<IActeur, bool>>>()))
            .ReturnsAsync(false);
        ActeurRepositoryMock.Setup(r => r.AjouterAsync(It.IsAny<IActeur>()))
            .ReturnsAsync(mockActeur);

        // Act
        Guid nouvActeur = await Service.CreerActeur(PrenomValide, NomValide);

        // Assert
        Assert.That(nouvActeur, Is.EqualTo(mockActeur.Id));
    }

    [Test]
    public async Task CreerActeur_WhenActeurIsUniqueAndValid_ShouldPersistNewActeur()
    {
        // Arrange
        ActeurRepositoryMock.Setup(r => r.ExisteAsync(It.IsAny<Expression<Func<IActeur, bool>>>()))
            .ReturnsAsync(false);
        ActeurRepositoryMock.Setup(r => r.AjouterAsync(It.IsAny<IActeur>()))
            .ReturnsAsync(Mock.Of<IActeur>(a => a.Id == Guid.NewGuid()));

        // Act
        _ = await Service.CreerActeur(PrenomValide, NomValide);

        // Assert
        ActeurRepositoryMock.Verify(r => r.AjouterAsync(It.IsAny<IActeur>()), Times.Once);
        UnitOfWorkMock.Verify(uow => uow.SauvegarderAsync(It.IsAny<CancellationToken?>()), Times.Once);
    }

    [Test]
    public async Task CreerActeur_WhenGivenNomWithLeadingOrTrailingSpaces_ShouldTrimNom()
    {
        // Arrange
        ActeurRepositoryMock.Setup(r => r.ExisteAsync(It.IsAny<Expression<Func<IActeur, bool>>>()))
            .ReturnsAsync(false);
        ActeurRepositoryMock.Setup(r => r.AjouterAsync(It.IsAny<IActeur>()))
            .ReturnsAsync(Mock.Of<IActeur>(a => a.Id == Guid.NewGuid()));

        // Act
        _ = await Service.CreerActeur(PrenomValide, $" {NomValide} ");

        // Assert
        ActeurRepositoryMock.Verify(r => r.AjouterAsync(It.Is<IActeur>(a => a.Nom == NomValide)));
    }

    [Test]
    public void CreerActeur_WhenGivenNomWithOnlySpaces_ShouldThrowAggregateExceptionContainingArgumentException()
    {
        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            Service.CreerActeur(PrenomValide, "   "));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("ne doit pas être vide"));
    }

    [Test]
    public async Task CreerActeur_WhenGivenPrenomWithLeadingOrTrailingSpaces_ShouldTrimPrenom()
    {
        // Arrange
        ActeurRepositoryMock.Setup(r => r.ExisteAsync(It.IsAny<Expression<Func<IActeur, bool>>>()))
            .ReturnsAsync(false);
        ActeurRepositoryMock.Setup(r => r.AjouterAsync(It.IsAny<IActeur>()))
            .ReturnsAsync(Mock.Of<IActeur>(a => a.Id == Guid.NewGuid()));

        // Act
        _ = await Service.CreerActeur($" {PrenomValide} ", NomValide);

        // Assert
        ActeurRepositoryMock.Verify(r => r.AjouterAsync(It.Is<IActeur>(a => a.Prenom == PrenomValide)));
    }

    [Test]
    public void CreerActeur_WhenGivenPrenomWithOnlySpaces_ShouldThrowAggregateExceptionContainingArgumentException()
    {
        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            Service.CreerActeur("   ", NomValide));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("ne doit pas être vide"));
    }

    [Test]
    public void
        CreerActeur_WhenOtherActeurWithSameNomAndPrenomAlreadyExists_ShouldThrowAggregateExceptionContainingArgumentException()
    {
        // Arrange
        ActeurRepositoryMock.Setup(r => r.ExisteAsync(It.IsAny<Expression<Func<IActeur, bool>>>()))
            .ReturnsAsync(true);

        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            Service.CreerActeur(PrenomValide, NomValide));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("existe déjà"));
    }
}