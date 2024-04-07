using System.Linq.Expressions;

using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Repositories;
using CineQuebec.Application.Services;
using CineQuebec.Domain.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Films;

using Moq;

namespace Tests.Application.Services;

public class FilmCreationServiceTests
{
    private const string TitreValide = "Le Seigneur des Anneaux";
    private const string AutreTitreValide = "Trouver Nemo";
    private const string DescriptionValide = "Un film de Peter Jackson";
    private const ushort DureeValide = 178;
    private readonly Guid _idCategorieValide = Guid.NewGuid();

    private Mock<IUnitOfWorkFactory> _unitOfWorkFactoryMock = null!;
    private IUnitOfWorkFactory _unitOfWorkFactory = null!;
    private Mock<IUnitOfWork> _unitOfWorkMock = null!;
    private IUnitOfWork _unitOfWork = null!;
    private Mock<IRepository<IFilm>> _filmRepositoryMock = null!;
    private IRepository<IFilm> _filmRepository = null!;
    private Mock<IRepository<IActeur>> _acteurRepositoryMock = null!;
    private IRepository<IActeur> _acteurRepository = null!;
    private Mock<IRepository<IRealisateur>> _realisateurRepositoryMock = null!;
    private IRepository<IRealisateur> _realisateurRepository = null!;
    private Mock<IRepository<ICategorieFilm>> _categorieFilmRepositoryMock = null!;
    private IRepository<ICategorieFilm> _categorieFilmRepository = null!;
    private FilmCreationService _service = null!;

    [Test]
    public void CreerFilm_WhenGivenActeurNotPresentInRepository_ThrowsAggregateExceptionContainingArgumentException()
    {
        // Arrange
        _acteurRepositoryMock.Setup(r => r.ObtenirParIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Acteur?)null);

        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            _service.CreerFilm(TitreValide, DescriptionValide, _idCategorieValide, DateTime.Now,
                new[] { Guid.NewGuid() }, Array.Empty<Guid>(), DureeValide));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("n'existe pas"));
    }

    [Test]
    public void CreerFilm_WhenGivenCategorieNotPresentInRepository_ThrowsAggregateExceptionContainingArgumentException()
    {
        // Arrange
        _categorieFilmRepositoryMock.Setup(r => r.ObtenirParIdAsync(_idCategorieValide))
            .ReturnsAsync((CategorieFilm?)null);

        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            _service.CreerFilm(TitreValide, DescriptionValide, _idCategorieValide, DateTime.Now, Array.Empty<Guid>(),
                Array.Empty<Guid>(), DureeValide));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("n'existe pas"));
    }

    [Test]
    public void CreerFilm_WhenGivenInvalidDescription_ThrowsAggregateExceptionContainingArgumentException()
    {
        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            _service.CreerFilm(TitreValide, "", _idCategorieValide, DateTime.Now, Array.Empty<Guid>(),
                Array.Empty<Guid>(), DureeValide));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("description ne peut pas être vide"));
    }

    [Test]
    public void CreerFilm_WhenGivenInvalidDuration_ThrowsAggregateExceptionContainingArgumentException()
    {
        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            _service.CreerFilm(TitreValide, DescriptionValide, _idCategorieValide, DateTime.Now, Array.Empty<Guid>(),
                Array.Empty<Guid>(), 0));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("doit durer plus de 0 minutes"));
    }

    [Test]
    public void CreerFilm_WhenGivenInvalidReleaseDate_ThrowsAggregateExceptionContainingArgumentException()
    {
        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            _service.CreerFilm(TitreValide, DescriptionValide, _idCategorieValide, DateTime.MinValue,
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
            _service.CreerFilm("", DescriptionValide, _idCategorieValide, DateTime.Now, Array.Empty<Guid>(),
                Array.Empty<Guid>(), DureeValide));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("titre ne peut pas être vide"));
    }

    [Test]
    public void
        CreerFilm_WhenGivenRealisateurNotPresentInRepository_ThrowsAggregateExceptionContainingArgumentException()
    {
        // Arrange
        _realisateurRepositoryMock.Setup(r => r.ObtenirParIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Realisateur?)null);

        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            _service.CreerFilm(TitreValide, DescriptionValide, _idCategorieValide, DateTime.Now, Array.Empty<Guid>(),
                new[] { Guid.NewGuid() }, DureeValide));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("n'existe pas"));
    }

    [Test]
    public void
        CreerFilm_WhenGivenTitleAndYearAndDurationAlreadyPresentInRepository_ThrowsAggregateExceptionContainingArgumentException()
    {
        // Arrange
        _filmRepositoryMock.Setup(r => r.ExisteAsync(It.IsAny<Expression<Func<IFilm, bool>>>()))
            .ReturnsAsync(true);

        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            _service.CreerFilm(TitreValide, DescriptionValide, _idCategorieValide, DateTime.Now, Array.Empty<Guid>(),
                Array.Empty<Guid>(), DureeValide));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("existe déjà"));
    }

    [Test]
    public async Task CreerFilm_WhenGivenValidArguments_CallsUnitOfWorkSauvegarderAsync()
    {
        // Arrange
        _filmRepositoryMock.Setup(r => r.ExisteAsync(It.IsAny<Expression<Func<IFilm, bool>>>()))
            .ReturnsAsync(false);
        _filmRepositoryMock.Setup(r => r.AjouterAsync(It.IsAny<Film>()))
            .ReturnsAsync(Mock.Of<IFilm>(f => f.Id == Guid.NewGuid()));

        // Act
        await _service.CreerFilm(TitreValide, DescriptionValide, _idCategorieValide, DateTime.Now,
            Array.Empty<Guid>(), Array.Empty<Guid>(), DureeValide);

        // Assert
        _unitOfWorkMock.Verify(u => u.SauvegarderAsync(null), Times.Once);
    }

    [Test]
    public async Task CreerFilm_WhenGivenValidArguments_ReturnsNewFilmId()
    {
        // Arrange
        Guid idFilm = Guid.NewGuid();
        _filmRepositoryMock.Setup(r => r.ExisteAsync(It.IsAny<Expression<Func<IFilm, bool>>>()))
            .ReturnsAsync(false);
        _filmRepositoryMock.Setup(r => r.AjouterAsync(It.IsAny<Film>()))
            .ReturnsAsync(Mock.Of<IFilm>(f => f.Id == idFilm));

        // Act
        Guid result = await _service.CreerFilm(TitreValide, DescriptionValide, _idCategorieValide, DateTime.Now,
            Array.Empty<Guid>(), Array.Empty<Guid>(), DureeValide);

        // Assert
        Assert.That(idFilm, Is.EqualTo(result));
    }

    [SetUp]
    public void SetUp()
    {
        _unitOfWorkFactoryMock = new Mock<IUnitOfWorkFactory>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _filmRepositoryMock = new Mock<IRepository<IFilm>>();
        _acteurRepositoryMock = new Mock<IRepository<IActeur>>();
        _realisateurRepositoryMock = new Mock<IRepository<IRealisateur>>();
        _categorieFilmRepositoryMock = new Mock<IRepository<ICategorieFilm>>();

        _unitOfWorkFactory = _unitOfWorkFactoryMock.Object;
        _unitOfWork = _unitOfWorkMock.Object;
        _filmRepository = _filmRepositoryMock.Object;
        _acteurRepository = _acteurRepositoryMock.Object;
        _realisateurRepository = _realisateurRepositoryMock.Object;
        _categorieFilmRepository = _categorieFilmRepositoryMock.Object;

        _categorieFilmRepositoryMock.Setup(r => r.ObtenirParIdAsync(_idCategorieValide))
            .ReturnsAsync(Mock.Of<ICategorieFilm>(cf => cf.Id == _idCategorieValide));

        _unitOfWorkMock.Setup(u => u.FilmRepository).Returns(_filmRepository);
        _unitOfWorkMock.Setup(u => u.ActeurRepository).Returns(_acteurRepository);
        _unitOfWorkMock.Setup(u => u.RealisateurRepository).Returns(_realisateurRepository);
        _unitOfWorkMock.Setup(u => u.CategorieFilmRepository).Returns(_categorieFilmRepository);
        _unitOfWorkFactoryMock.Setup(f => f.Create()).Returns(_unitOfWork);

        _service = new FilmCreationService(_unitOfWorkFactory);
    }
}