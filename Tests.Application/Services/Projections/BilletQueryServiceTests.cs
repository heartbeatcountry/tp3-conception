using System.Linq.Expressions;

using CineQuebec.Application.Records.Projections;
using CineQuebec.Application.Services.Projections;
using CineQuebec.Domain.Interfaces.Entities.Projections;

using Moq;

namespace Tests.Application.Services.Projections;

public class BilletQueryServiceTests : GenericServiceTests<BilletQueryService>
{
    [Test]
    public async Task ObtenirTous_WhenBilletsExist_ShouldReturnAllBillets()
    {
        // Arrange
        IBillet[] billets =
        {
        Mock.Of<IBillet>(b => b.Id == Guid.NewGuid()),
        Mock.Of<IBillet>(b => b.Id == Guid.NewGuid()),
        Mock.Of<IBillet>(b => b.Id == Guid.NewGuid())
    };
        BilletRepositoryMock.Setup(r => r.ObtenirTousAsync(It.IsAny<Expression<Func<IBillet, bool>>>(), null))
            .ReturnsAsync(billets);

        // Act
        IEnumerable<BilletDto> billetsDto = (await Service.ObtenirTous()).ToArray();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(billetsDto, Has.Exactly(3).Items);
            Assert.That(billetsDto.ElementAt(0).Id, Is.EqualTo(billets[0].Id));
            Assert.That(billetsDto.ElementAt(1).Id, Is.EqualTo(billets[1].Id));
            Assert.That(billetsDto.ElementAt(2).Id, Is.EqualTo(billets[2].Id));
        });
    }

    [Test]
    public async Task ObtenirTous_WhenNoBilletExists_ShouldReturnEmptyList()
    {
        // Arrange
        IEnumerable<IBillet> billets = [];
        BilletRepositoryMock.Setup(r => r.ObtenirTousAsync(null, null)).ReturnsAsync(billets);

        // Act
        IEnumerable<BilletDto> billetsDto = await Service.ObtenirTous();

        // Assert
        Assert.That(billetsDto, Is.Empty);
    }
}