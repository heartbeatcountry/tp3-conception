using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Repositories;
using CineQuebec.Domain.Interfaces.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Projections;

using Moq;

namespace Tests.Application.Services;

public abstract class GenericServiceTests<TService> where TService : class
{
    private IRepository<IActeur> _acteurRepository = null!;
    private IRepository<ICategorieFilm> _categorieFilmRepository = null!;
    private IRepository<IFilm> _filmRepository = null!;
    private IRepository<IProjection> _projectionRepository = null!;
    private IRepository<IRealisateur> _realisateurRepository = null!;
    private IRepository<ISalle> _salleRepository = null!;
    private IUnitOfWork _unitOfWork = null!;
    private IUnitOfWorkFactory _unitOfWorkFactory = null!;

    protected TService Service { get; private set; } = null!;
    private Mock<IUnitOfWorkFactory> UnitOfWorkFactoryMock { get; set; } = null!;
    protected Mock<IUnitOfWork> UnitOfWorkMock { get; private set; } = null!;
    protected Mock<IRepository<IFilm>> FilmRepositoryMock { get; private set; } = null!;
    protected Mock<IRepository<IActeur>> ActeurRepositoryMock { get; private set; } = null!;
    protected Mock<IRepository<IRealisateur>> RealisateurRepositoryMock { get; private set; } = null!;
    protected Mock<IRepository<ICategorieFilm>> CategorieFilmRepositoryMock { get; private set; } = null!;
    protected Mock<IRepository<ISalle>> SalleRepositoryMock { get; private set; } = null!;
    protected Mock<IRepository<IProjection>> ProjectionRepositoryMock { get; private set; } = null!;

    [SetUp]
    public void SetUp()
    {
        UnitOfWorkFactoryMock = new Mock<IUnitOfWorkFactory>();
        UnitOfWorkMock = new Mock<IUnitOfWork>();
        FilmRepositoryMock = new Mock<IRepository<IFilm>>();
        ActeurRepositoryMock = new Mock<IRepository<IActeur>>();
        RealisateurRepositoryMock = new Mock<IRepository<IRealisateur>>();
        CategorieFilmRepositoryMock = new Mock<IRepository<ICategorieFilm>>();
        SalleRepositoryMock = new Mock<IRepository<ISalle>>();
        ProjectionRepositoryMock = new Mock<IRepository<IProjection>>();

        _unitOfWorkFactory = UnitOfWorkFactoryMock.Object;
        _unitOfWork = UnitOfWorkMock.Object;
        _filmRepository = FilmRepositoryMock.Object;
        _acteurRepository = ActeurRepositoryMock.Object;
        _realisateurRepository = RealisateurRepositoryMock.Object;
        _categorieFilmRepository = CategorieFilmRepositoryMock.Object;
        _salleRepository = SalleRepositoryMock.Object;
        _projectionRepository = ProjectionRepositoryMock.Object;

        UnitOfWorkMock.Setup(u => u.FilmRepository).Returns(_filmRepository);
        UnitOfWorkMock.Setup(u => u.ActeurRepository).Returns(_acteurRepository);
        UnitOfWorkMock.Setup(u => u.RealisateurRepository).Returns(_realisateurRepository);
        UnitOfWorkMock.Setup(u => u.CategorieFilmRepository).Returns(_categorieFilmRepository);
        UnitOfWorkMock.Setup(u => u.SalleRepository).Returns(_salleRepository);
        UnitOfWorkMock.Setup(u => u.ProjectionRepository).Returns(_projectionRepository);
        UnitOfWorkFactoryMock.Setup(f => f.Create()).Returns(_unitOfWork);

        SetUpExt();

        Service = CreateInstance(_unitOfWorkFactory);
    }

    protected virtual void SetUpExt() { }

    private static TService CreateInstance(params object?[] args)
    {
        return (TService)Activator.CreateInstance(typeof(TService), args)!;
    }
}