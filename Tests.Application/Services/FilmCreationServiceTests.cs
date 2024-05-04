using System.Linq.Expressions;

using CineQuebec.Application.Services;
using CineQuebec.Domain.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Films;

using Moq;

namespace Tests.Application.Services;

public class FilmCreationServiceTests : GenericServiceTests<FilmCreationService>
{
    private const string TitreValide = "Le Seigneur des Anneaux";
    private const string DescriptionValide = "Un film de Peter Jackson";
    private const ushort DureeValide = 178;
    private readonly Guid _idCategorieValide = Guid.NewGuid();

    [Test]
    public void
        CreerFilm_WhenGivenActeurNotPresentInRepository_ShouldThrowAggregateExceptionContainingArgumentException()
    {
        // Arrange
        ActeurRepositoryMock.Setup(r => r.ObtenirParIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Acteur?)null);

        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            Service.CreerFilm(TitreValide, DescriptionValide, _idCategorieValide, DateTime.Now,
                [Guid.NewGuid()], [], DureeValide));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("n'existe pas"));
    }

    [Test]
    public void
        CreerFilm_WhenGivenCategorieNotPresentInRepository_ShouldThrowAggregateExceptionContainingArgumentException()
    {
        // Arrange
        CategorieFilmRepositoryMock.Setup(r => r.ObtenirParIdAsync(_idCategorieValide))
            .ReturnsAsync((CategorieFilm?)null);

        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            Service.CreerFilm(TitreValide, DescriptionValide, _idCategorieValide, DateTime.Now, [],
                [], DureeValide));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("n'existe pas"));
    }

    [Test]
    public void CreerFilm_WhenGivenInvalidDescription_ShouldThrowAggregateExceptionContainingArgumentException()
    {
        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            Service.CreerFilm(TitreValide, "", _idCategorieValide, DateTime.Now, [],
                [], DureeValide));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("description ne peut pas être vide"));
    }

    [Test]
    public void CreerFilm_WhenGivenInvalidDuration_ShouldThrowAggregateExceptionContainingArgumentException()
    {
        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            Service.CreerFilm(TitreValide, DescriptionValide, _idCategorieValide, DateTime.Now, [],
                [], 0));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("doit durer plus de 0 minutes"));
    }

    [Test]
    public void CreerFilm_WhenGivenInvalidReleaseDate_ShouldThrowAggregateExceptionContainingArgumentException()
    {
        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            Service.CreerFilm(TitreValide, DescriptionValide, _idCategorieValide, DateTime.MinValue,
                [], [], DureeValide));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message
                .Contains("date de sortie internationale doit être supérieure à"));
    }

    [Test]
    public void CreerFilm_WhenGivenInvalidTitle_ShouldThrowAggregateExceptionContainingArgumentException()
    {
        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            Service.CreerFilm("", DescriptionValide, _idCategorieValide, DateTime.Now, [],
                [], DureeValide));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("titre ne peut pas être vide"));
    }

    [Test]
    public void
        CreerFilm_WhenGivenRealisateurNotPresentInRepository_ShouldThrowAggregateExceptionContainingArgumentException()
    {
        // Arrange
        RealisateurRepositoryMock.Setup(r => r.ObtenirParIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Realisateur?)null);

        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            Service.CreerFilm(TitreValide, DescriptionValide, _idCategorieValide, DateTime.Now, [],
                [Guid.NewGuid()], DureeValide));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("n'existe pas"));
    }

    [Test]
    public void
        CreerFilm_WhenGivenTitleAndYearAndDurationAlreadyPresentInRepository_ShouldThrowAggregateExceptionContainingArgumentException()
    {
        // Arrange
        FilmRepositoryMock.Setup(r => r.ExisteAsync(It.IsAny<Expression<Func<IFilm, bool>>>()))
            .ReturnsAsync(true);

        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            Service.CreerFilm(TitreValide, DescriptionValide, _idCategorieValide, DateTime.Now, [],
                [], DureeValide));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("existe déjà"));
    }

    [Test]
    public async Task CreerFilm_WhenGivenValidArguments_ShouldCreateAndPersistNewFilm()
    {
        // Arrange
        IFilm mockFilm = Mock.Of<IFilm>(f => f.Id == Guid.NewGuid());
        FilmRepositoryMock.Setup(r => r.ExisteAsync(It.IsAny<Expression<Func<IFilm, bool>>>()))
            .ReturnsAsync(false);
        FilmRepositoryMock.Setup(r => r.AjouterAsync(It.IsAny<Film>()))
            .ReturnsAsync(mockFilm);

        // Act
        _ = await Service.CreerFilm(TitreValide, DescriptionValide, _idCategorieValide, DateTime.Now,
            [], [], DureeValide);

        // Assert
        FilmRepositoryMock.Verify(r => r.AjouterAsync(It.IsAny<Film>()), Times.Once);
        UnitOfWorkMock.Verify(uow => uow.SauvegarderAsync(It.IsAny<CancellationToken?>()), Times.Once);
    }

    [Test]
    public async Task CreerFilm_WhenGivenValidArguments_ShouldCreateAndReturnNewFilm()
    {
        // Arrange
        IFilm mockFilm = Mock.Of<IFilm>(f => f.Id == Guid.NewGuid());
        FilmRepositoryMock.Setup(r => r.ExisteAsync(It.IsAny<Expression<Func<IFilm, bool>>>()))
            .ReturnsAsync(false);
        FilmRepositoryMock.Setup(r => r.AjouterAsync(It.IsAny<Film>()))
            .ReturnsAsync(mockFilm);

        // Act
        Guid nouvFilm = await Service.CreerFilm(TitreValide, DescriptionValide, _idCategorieValide, DateTime.Now,
            [], [], DureeValide);

        // Assert
        Assert.That(nouvFilm, Is.EqualTo(mockFilm.Id));
    }

    [SetUp]
    public new void SetUp()
    {
        base.SetUp();

        CategorieFilmRepositoryMock.Setup(r => r.ObtenirParIdAsync(_idCategorieValide))
            .ReturnsAsync(Mock.Of<ICategorieFilm>(cf => cf.Id == _idCategorieValide));
    }
}