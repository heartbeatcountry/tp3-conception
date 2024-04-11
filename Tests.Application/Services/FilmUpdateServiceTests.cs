using System.Linq.Expressions;

using CineQuebec.Application.Services;
using CineQuebec.Domain.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Films;

using Moq;

namespace Tests.Application.Services;

public class FilmUpdateServiceTests : GenericServiceTests<FilmUpdateService>
{
    private const string TitreValide = "Le Seigneur des Anneaux";
    private const string DescriptionValide = "Un film de Peter Jackson";
    private const ushort DureeValide = 178;
    private readonly Guid _idCategorieValide = Guid.NewGuid();
    private readonly Guid _idFilmValide = Guid.NewGuid();

    [Test]
    public void ModifierFilm_WhenGivenActeurNotPresentInRepository_ThrowsAggregateExceptionContainingArgumentException()
    {
        // Arrange
        ActeurRepositoryMock.Setup(r => r.ObtenirParIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Acteur?)null);

        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            Service.ModifierFilm(_idFilmValide, TitreValide, DescriptionValide, _idCategorieValide, DateTime.Now,
                new[] { Guid.NewGuid() }, Array.Empty<Guid>(), DureeValide));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("n'existe pas"));
    }

    [Test]
    public void
        ModifierFilm_WhenGivenCategorieNotPresentInRepository_ThrowsAggregateExceptionContainingArgumentException()
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
    public void ModifierFilm_WhenGivenInvalidDescription_ThrowsAggregateExceptionContainingArgumentException()
    {
        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            Service.ModifierFilm(_idFilmValide, TitreValide, "", _idCategorieValide, DateTime.Now, Array.Empty<Guid>(),
                Array.Empty<Guid>(), DureeValide));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("description ne peut pas être vide"));
    }

    [Test]
    public void ModifierFilm_WhenGivenInvalidDuration_ThrowsAggregateExceptionContainingArgumentException()
    {
        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            Service.ModifierFilm(_idFilmValide, TitreValide, DescriptionValide, _idCategorieValide, DateTime.Now,
                Array.Empty<Guid>(), Array.Empty<Guid>(), 0));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("doit durer plus de 0 minutes"));
    }

    [Test]
    public void ModifierFilm_WhenGivenInvalidId_ThrowsAggregateExceptionContainingArgumentException()
    {
        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            Service.ModifierFilm(Guid.NewGuid(), TitreValide, DescriptionValide, _idCategorieValide, DateTime.Now,
                Array.Empty<Guid>(), Array.Empty<Guid>(), DureeValide));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("n'existe pas"));
    }

    [Test]
    public void ModifierFilm_WhenGivenInvalidReleaseDate_ThrowsAggregateExceptionContainingArgumentException()
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
    public void ModifierFilm_WhenGivenInvalidTitle_ThrowsAggregateExceptionContainingArgumentException()
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
        ModifierFilm_WhenGivenRealisateurNotPresentInRepository_ThrowsAggregateExceptionContainingArgumentException()
    {
        // Arrange
        RealisateurRepositoryMock.Setup(r => r.ObtenirParIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Realisateur?)null);

        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            Service.ModifierFilm(_idFilmValide, TitreValide, DescriptionValide, _idCategorieValide, DateTime.Now,
                Array.Empty<Guid>(), new[] { Guid.NewGuid() }, DureeValide));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("n'existe pas"));
    }

    [Test]
    public void
        ModifierFilm_WhenGivenTitleAndYearAndDurationAlreadyPresentInRepository_ThrowsAggregateExceptionContainingArgumentException()
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
    public async Task ModifierFilm_WhenGivenValidArguments_CallsUnitOfWorkSauvegarderAsync()
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
        UnitOfWorkMock.Verify(u => u.SauvegarderAsync(null), Times.Once);
    }

    protected override void SetUpExt()
    {
        CategorieFilmRepositoryMock.Setup(r => r.ObtenirParIdAsync(_idCategorieValide))
            .ReturnsAsync(Mock.Of<ICategorieFilm>(cf => cf.Id == _idCategorieValide));

        FilmRepositoryMock.Setup(r => r.ObtenirParIdAsync(_idFilmValide))
            .ReturnsAsync(Mock.Of<IFilm>(f => f.Id == _idFilmValide));
    }
}