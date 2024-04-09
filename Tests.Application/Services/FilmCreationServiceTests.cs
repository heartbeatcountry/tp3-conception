using System.Linq.Expressions;

using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Repositories;
using CineQuebec.Application.Services;
using CineQuebec.Domain.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Films;

using Moq;

namespace Tests.Application.Services;

public class FilmCreationServiceTests: GenericServiceTests<FilmCreationService>
{
    private const string TitreValide = "Le Seigneur des Anneaux";
    private const string DescriptionValide = "Un film de Peter Jackson";
    private const ushort DureeValide = 178;
    private readonly Guid _idCategorieValide = Guid.NewGuid();

    protected override void SetUpExt()
    {
        CategorieFilmRepositoryMock.Setup(r => r.ObtenirParIdAsync(_idCategorieValide))
            .ReturnsAsync(Mock.Of<ICategorieFilm>(cf => cf.Id == _idCategorieValide));
    }

    [Test]
    public void CreerFilm_WhenGivenActeurNotPresentInRepository_ThrowsAggregateExceptionContainingArgumentException()
    {
        // Arrange
        ActeurRepositoryMock.Setup(r => r.ObtenirParIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Acteur?)null);

        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            Service.CreerFilm(TitreValide, DescriptionValide, _idCategorieValide, DateTime.Now,
                new[] { Guid.NewGuid() }, Array.Empty<Guid>(), DureeValide));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("n'existe pas"));
    }

    [Test]
    public void CreerFilm_WhenGivenCategorieNotPresentInRepository_ThrowsAggregateExceptionContainingArgumentException()
    {
        // Arrange
        CategorieFilmRepositoryMock.Setup(r => r.ObtenirParIdAsync(_idCategorieValide))
            .ReturnsAsync((CategorieFilm?)null);

        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            Service.CreerFilm(TitreValide, DescriptionValide, _idCategorieValide, DateTime.Now, Array.Empty<Guid>(),
                Array.Empty<Guid>(), DureeValide));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("n'existe pas"));
    }

    [Test]
    public void CreerFilm_WhenGivenInvalidDescription_ThrowsAggregateExceptionContainingArgumentException()
    {
        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            Service.CreerFilm(TitreValide, "", _idCategorieValide, DateTime.Now, Array.Empty<Guid>(),
                Array.Empty<Guid>(), DureeValide));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("description ne peut pas être vide"));
    }

    [Test]
    public void CreerFilm_WhenGivenInvalidDuration_ThrowsAggregateExceptionContainingArgumentException()
    {
        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            Service.CreerFilm(TitreValide, DescriptionValide, _idCategorieValide, DateTime.Now, Array.Empty<Guid>(),
                Array.Empty<Guid>(), 0));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("doit durer plus de 0 minutes"));
    }

    [Test]
    public void CreerFilm_WhenGivenInvalidReleaseDate_ThrowsAggregateExceptionContainingArgumentException()
    {
        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            Service.CreerFilm(TitreValide, DescriptionValide, _idCategorieValide, DateTime.MinValue,
                Array.Empty<Guid>(), Array.Empty<Guid>(), DureeValide));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message
                .Contains("date de sortie internationale doit être supérieure à"));
    }

    [Test]
    public void CreerFilm_WhenGivenInvalidTitle_ThrowsAggregateExceptionContainingArgumentException()
    {
        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            Service.CreerFilm("", DescriptionValide, _idCategorieValide, DateTime.Now, Array.Empty<Guid>(),
                Array.Empty<Guid>(), DureeValide));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("titre ne peut pas être vide"));
    }

    [Test]
    public void
        CreerFilm_WhenGivenRealisateurNotPresentInRepository_ThrowsAggregateExceptionContainingArgumentException()
    {
        // Arrange
        RealisateurRepositoryMock.Setup(r => r.ObtenirParIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Realisateur?)null);

        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            Service.CreerFilm(TitreValide, DescriptionValide, _idCategorieValide, DateTime.Now, Array.Empty<Guid>(),
                new[] { Guid.NewGuid() }, DureeValide));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("n'existe pas"));
    }

    [Test]
    public void
        CreerFilm_WhenGivenTitleAndYearAndDurationAlreadyPresentInRepository_ThrowsAggregateExceptionContainingArgumentException()
    {
        // Arrange
        FilmRepositoryMock.Setup(r => r.ExisteAsync(It.IsAny<Expression<Func<IFilm, bool>>>()))
            .ReturnsAsync(true);

        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            Service.CreerFilm(TitreValide, DescriptionValide, _idCategorieValide, DateTime.Now, Array.Empty<Guid>(),
                Array.Empty<Guid>(), DureeValide));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("existe déjà"));
    }

    [Test]
    public async Task CreerFilm_WhenGivenValidArguments_CallsUnitOfWorkSauvegarderAsync()
    {
        // Arrange
        FilmRepositoryMock.Setup(r => r.ExisteAsync(It.IsAny<Expression<Func<IFilm, bool>>>()))
            .ReturnsAsync(false);
        FilmRepositoryMock.Setup(r => r.AjouterAsync(It.IsAny<Film>()))
            .ReturnsAsync(Mock.Of<IFilm>(f => f.Id == Guid.NewGuid()));

        // Act
        await Service.CreerFilm(TitreValide, DescriptionValide, _idCategorieValide, DateTime.Now,
            Array.Empty<Guid>(), Array.Empty<Guid>(), DureeValide);

        // Assert
        UnitOfWorkMock.Verify(u => u.SauvegarderAsync(null), Times.Once);
    }

    [Test]
    public async Task CreerFilm_WhenGivenValidArguments_ReturnsNewFilmId()
    {
        // Arrange
        Guid idFilm = Guid.NewGuid();
        FilmRepositoryMock.Setup(r => r.ExisteAsync(It.IsAny<Expression<Func<IFilm, bool>>>()))
            .ReturnsAsync(false);
        FilmRepositoryMock.Setup(r => r.AjouterAsync(It.IsAny<Film>()))
            .ReturnsAsync(Mock.Of<IFilm>(f => f.Id == idFilm));

        // Act
        Guid result = await Service.CreerFilm(TitreValide, DescriptionValide, _idCategorieValide, DateTime.Now,
            Array.Empty<Guid>(), Array.Empty<Guid>(), DureeValide);

        // Assert
        Assert.That(idFilm, Is.EqualTo(result));
    }
}