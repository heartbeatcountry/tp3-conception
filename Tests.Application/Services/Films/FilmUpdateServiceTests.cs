using System.Linq.Expressions;

using CineQuebec.Application.Services.Films;
using CineQuebec.Domain.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Films;

using Moq;

namespace Tests.Application.Services.Films;

public class FilmUpdateServiceTests : GenericServiceTests<FilmUpdateService>
{
    private const string TitreValide = "Le Seigneur des Anneaux";
    private const string DescriptionValide = "Un film de Peter Jackson";
    private const ushort DureeValide = 178;
    private readonly Guid _idCategorieValide = Guid.NewGuid();
    private readonly Guid _idFilmValide = Guid.NewGuid();

    [Test]
    public void
        ModifierFilm_WhenGivenActeurNotPresentInRepository_ShouldThrowAggregateExceptionContainingArgumentException()
    {
        // Arrange
        ActeurRepositoryMock.Setup(r => r.ObtenirParIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Acteur?)null);

        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            Service.ModifierFilm(_idFilmValide, TitreValide, DescriptionValide, _idCategorieValide, DateTime.Now,
                [Guid.NewGuid()], Array.Empty<Guid>(), DureeValide));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("n'existe pas"));
    }

    [Test]
    public void
        ModifierFilm_WhenGivenCategorieNotPresentInRepository_ShouldThrowAggregateExceptionContainingArgumentException()
    {
        // Arrange
        CategorieFilmRepositoryMock.Setup(r => r.ObtenirParIdAsync(_idCategorieValide))
            .ReturnsAsync((CategorieFilm?)null);

        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            Service.ModifierFilm(_idFilmValide, TitreValide, DescriptionValide, _idCategorieValide, DateTime.Now,
                Array.Empty<Guid>(), Array.Empty<Guid>(), DureeValide));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("n'existe pas"));
    }

    [Test]
    public void ModifierFilm_WhenGivenInvalidDescription_ShouldThrowAggregateExceptionContainingArgumentException()
    {
        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            Service.ModifierFilm(_idFilmValide, TitreValide, "", _idCategorieValide, DateTime.Now, Array.Empty<Guid>(),
                Array.Empty<Guid>(), DureeValide));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("description ne peut pas être vide"));
    }

    [Test]
    public void ModifierFilm_WhenGivenInvalidDuration_ShouldThrowAggregateExceptionContainingArgumentException()
    {
        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            Service.ModifierFilm(_idFilmValide, TitreValide, DescriptionValide, _idCategorieValide, DateTime.Now,
                Array.Empty<Guid>(), Array.Empty<Guid>(), 0));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("doit durer plus de 0 minutes"));
    }

    [Test]
    public void ModifierFilm_WhenGivenInvalidId_ShouldThrowAggregateExceptionContainingArgumentException()
    {
        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            Service.ModifierFilm(Guid.NewGuid(), TitreValide, DescriptionValide, _idCategorieValide, DateTime.Now,
                Array.Empty<Guid>(), Array.Empty<Guid>(), DureeValide));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("n'existe pas"));
    }

    [Test]
    public void ModifierFilm_WhenGivenInvalidReleaseDate_ShouldThrowAggregateExceptionContainingArgumentException()
    {
        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            Service.ModifierFilm(_idFilmValide, TitreValide, DescriptionValide, _idCategorieValide, DateTime.MinValue,
                Array.Empty<Guid>(), Array.Empty<Guid>(), DureeValide));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message
                .Contains("date de sortie internationale doit être supérieure à"));
    }

    [Test]
    public void ModifierFilm_WhenGivenInvalidTitle_ShouldThrowAggregateExceptionContainingArgumentException()
    {
        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            Service.ModifierFilm(_idFilmValide, "", DescriptionValide, _idCategorieValide, DateTime.Now,
                Array.Empty<Guid>(), Array.Empty<Guid>(), DureeValide));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("titre ne peut pas être vide"));
    }

    [Test]
    public void
        ModifierFilm_WhenGivenRealisateurNotPresentInRepository_ShouldThrowAggregateExceptionContainingArgumentException()
    {
        // Arrange
        RealisateurRepositoryMock.Setup(r => r.ObtenirParIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Realisateur?)null);

        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            Service.ModifierFilm(_idFilmValide, TitreValide, DescriptionValide, _idCategorieValide, DateTime.Now,
                Array.Empty<Guid>(), [Guid.NewGuid()], DureeValide));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("n'existe pas"));
    }

    [Test]
    public void
        ModifierFilm_WhenGivenTitleAndYearAndDurationAlreadyPresentInRepository_ShouldThrowAggregateExceptionContainingArgumentException()
    {
        // Arrange
        FilmRepositoryMock.Setup(r => r.ExisteAsync(It.IsAny<Expression<Func<IFilm, bool>>>()))
            .ReturnsAsync(true);

        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            Service.ModifierFilm(_idFilmValide, TitreValide, DescriptionValide, _idCategorieValide, DateTime.Now,
                Array.Empty<Guid>(), Array.Empty<Guid>(), DureeValide));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("existe déjà"));
    }

    [Test]
    public async Task ModifierFilm_WhenGivenValidArguments_ShouldPersistChanges()
    {
        // Arrange
        FilmRepositoryMock.Setup(r => r.ExisteAsync(It.IsAny<Expression<Func<IFilm, bool>>>()))
            .ReturnsAsync(false);
        FilmRepositoryMock.Setup(r => r.AjouterAsync(It.IsAny<Film>()))
            .ReturnsAsync(Mock.Of<IFilm>(f => f.Id == Guid.NewGuid()));

        // Act
        await Service.ModifierFilm(_idFilmValide, TitreValide, DescriptionValide, _idCategorieValide, DateTime.Now,
            Array.Empty<Guid>(), Array.Empty<Guid>(), DureeValide);

        // Assert
        FilmRepositoryMock.Verify(r => r.Modifier(It.IsAny<IFilm>()), Times.Once);
        UnitOfWorkMock.Verify(u => u.SauvegarderAsync(null), Times.Once);
    }

    [SetUp]
    public new void SetUp()
    {
        base.SetUp();

        CategorieFilmRepositoryMock.Setup(r => r.ObtenirParIdAsync(_idCategorieValide))
            .ReturnsAsync(Mock.Of<ICategorieFilm>(cf => cf.Id == _idCategorieValide));

        FilmRepositoryMock.Setup(r => r.ObtenirParIdAsync(_idFilmValide))
            .ReturnsAsync(Mock.Of<IFilm>(f => f.Id == _idFilmValide));
    }
}