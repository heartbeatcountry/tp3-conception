using System.Linq.Expressions;

using CineQuebec.Application.Services;
using CineQuebec.Domain.Entities.Projections;
using CineQuebec.Domain.Interfaces.Entities.Projections;

using Moq;

namespace Tests.Application.Services;

public class SalleCreationServiceTests : GenericServiceTests<SalleCreationService>
{
    private const byte NumeroValide = 1;
    private const ushort NbSiegesValide = 100;

    [Test]
    public void CreerSalle_WhenInvalidNumberOfSeats_ShouldThrowAggregateExceptionContainingArgumentException()
    {
        // Arrange
        SalleRepositoryMock.Setup(r => r.ExisteAsync(It.IsAny<Expression<Func<ISalle, bool>>>()))
            .ReturnsAsync(false);
        SalleRepositoryMock.Setup(r => r.AjouterAsync(It.IsAny<Salle>()))
            .ReturnsAsync(Mock.Of<ISalle>(a => a.Id == Guid.NewGuid()));

        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            Service.CreerSalle(NumeroValide, 0));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("doit être supérieur à 0"));
    }

    [Test]
    public void CreerSalle_WhenInvalidNumero_ShouldThrowAggregateExceptionContainingArgumentException()
    {
        // Arrange
        SalleRepositoryMock.Setup(r => r.ExisteAsync(It.IsAny<Expression<Func<ISalle, bool>>>()))
            .ReturnsAsync(false);
        SalleRepositoryMock.Setup(r => r.AjouterAsync(It.IsAny<Salle>()))
            .ReturnsAsync(Mock.Of<ISalle>(a => a.Id == Guid.NewGuid()));

        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            Service.CreerSalle(0, NbSiegesValide));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("doit être supérieur à 0"));
    }

    [Test]
    public async Task CreerSalle_WhenSalleEstUnique_ShouldAddSalleToRepository()
    {
        // Arrange
        SalleRepositoryMock.Setup(r => r.ExisteAsync(It.IsAny<Expression<Func<ISalle, bool>>>()))
            .ReturnsAsync(false);
        SalleRepositoryMock.Setup(r => r.AjouterAsync(It.IsAny<Salle>()))
            .ReturnsAsync(Mock.Of<ISalle>(a => a.Id == Guid.NewGuid()));

        // Act
        await Service.CreerSalle(NumeroValide, NbSiegesValide);

        // Assert
        SalleRepositoryMock.Verify(r => r.AjouterAsync(It.IsAny<Salle>()));
    }

    [Test]
    public async Task CreerSalle_WhenSalleEstUnique_ShouldReturnGuidOfNewSalle()
    {
        // Arrange
        SalleRepositoryMock.Setup(r => r.ExisteAsync(It.IsAny<Expression<Func<ISalle, bool>>>()))
            .ReturnsAsync(false);
        SalleRepositoryMock.Setup(r => r.AjouterAsync(It.IsAny<Salle>()))
            .ReturnsAsync(Mock.Of<ISalle>(a => a.Id == Guid.NewGuid()));

        // Act
        Guid salleId = await Service.CreerSalle(NumeroValide, NbSiegesValide);

        // Assert
        Assert.That(salleId, Is.Not.EqualTo(Guid.Empty));
    }

    [Test]
    public void
        CreerSalle_WhenSalleWithSameNumeroAlreadyExists_ShouldThrowAggregateExceptionContainingArgumentException()
    {
        // Arrange
        SalleRepositoryMock.Setup(r => r.ExisteAsync(It.IsAny<Expression<Func<ISalle, bool>>>()))
            .ReturnsAsync(true);

        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            Service.CreerSalle(NumeroValide, NbSiegesValide));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("existe déjà"));
    }
}