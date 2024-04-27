using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Repositories;
using CineQuebec.Application.Interfaces.Services;
using CineQuebec.Domain.Interfaces.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Projections;
using CineQuebec.Domain.Interfaces.Entities.Utilisateur;

using Microsoft.Extensions.DependencyInjection;

using Moq;

namespace Tests.Application.Services;

public abstract class GenericServiceTests<TService> where TService : class
{
    private IRepository<IActeur> _acteurRepository = null!;
    private IRepository<ICategorieFilm> _categorieFilmRepository = null!;
    private IRepository<IFilm> _filmRepository = null!;
    private IPasswordHashingService _passwordHashingService = null!;
    private IPasswordValidationService _passwordValidationService = null!;
    private IRepository<INoteFilm> _noteFilmRepository = null!;
    private IRepository<IProjection> _projectionRepository = null!;
    private IRepository<IRealisateur> _realisateurRepository = null!;
    private IRepository<ISalle> _salleRepository = null!;
    private IUnitOfWork _unitOfWork = null!;
    private IUnitOfWorkFactory _unitOfWorkFactory = null!;
    private IUtilisateurAuthenticationService _utilisateurAuthenticationService = null!;
    private IUtilisateurCreationService _utilisateurCreationService = null!;
    private IRepository<IUtilisateur> _utilisateurRepository = null!;

    protected TService Service { get; private set; } = null!;
    private Mock<IUnitOfWorkFactory> UnitOfWorkFactoryMock { get; set; } = null!;
    protected Mock<IUnitOfWork> UnitOfWorkMock { get; private set; } = null!;

    protected Mock<IUtilisateurAuthenticationService> UtilisateurAuthenticationServiceMock { get; private set; } =
        null!;

    protected Mock<IUtilisateurCreationService> UtilisateurCreationServiceMock { get; private set; } = null!;
    protected Mock<IPasswordValidationService> PasswordValidationServiceMock { get; private set; } = null!;
    protected Mock<IPasswordHashingService> PasswordHashingServiceMock { get; private set; } = null!;
    protected Mock<IRepository<IFilm>> FilmRepositoryMock { get; private set; } = null!;
    protected Mock<IRepository<INoteFilm>> NoteFilmRepositoryMock { get; private set; } = null!;
    protected Mock<IRepository<IActeur>> ActeurRepositoryMock { get; private set; } = null!;
    protected Mock<IRepository<IRealisateur>> RealisateurRepositoryMock { get; private set; } = null!;
    protected Mock<IRepository<ICategorieFilm>> CategorieFilmRepositoryMock { get; private set; } = null!;
    protected Mock<IRepository<ISalle>> SalleRepositoryMock { get; private set; } = null!;
    protected Mock<IRepository<IProjection>> ProjectionRepositoryMock { get; private set; } = null!;
    protected Mock<IRepository<IUtilisateur>> UtilisateurRepositoryMock { get; private set; } = null!;

    [SetUp]
    public void SetUp()
    {
        UnitOfWorkFactoryMock = new Mock<IUnitOfWorkFactory>();
        UnitOfWorkMock = new Mock<IUnitOfWork>();
        FilmRepositoryMock = new Mock<IRepository<IFilm>>();
        NoteFilmRepositoryMock = new Mock<IRepository<INoteFilm>>();
        ActeurRepositoryMock = new Mock<IRepository<IActeur>>();
        RealisateurRepositoryMock = new Mock<IRepository<IRealisateur>>();
        CategorieFilmRepositoryMock = new Mock<IRepository<ICategorieFilm>>();
        SalleRepositoryMock = new Mock<IRepository<ISalle>>();
        ProjectionRepositoryMock = new Mock<IRepository<IProjection>>();
        UtilisateurRepositoryMock = new Mock<IRepository<IUtilisateur>>();
        UtilisateurAuthenticationServiceMock = new Mock<IUtilisateurAuthenticationService>();
        UtilisateurCreationServiceMock = new Mock<IUtilisateurCreationService>();
        PasswordValidationServiceMock = new Mock<IPasswordValidationService>();
        PasswordHashingServiceMock = new Mock<IPasswordHashingService>();

        _unitOfWorkFactory = UnitOfWorkFactoryMock.Object;
        _unitOfWork = UnitOfWorkMock.Object;
        _filmRepository = FilmRepositoryMock.Object;
        _noteFilmRepository = NoteFilmRepositoryMock.Object;
        _acteurRepository = ActeurRepositoryMock.Object;
        _realisateurRepository = RealisateurRepositoryMock.Object;
        _categorieFilmRepository = CategorieFilmRepositoryMock.Object;
        _salleRepository = SalleRepositoryMock.Object;
        _projectionRepository = ProjectionRepositoryMock.Object;
        _utilisateurRepository = UtilisateurRepositoryMock.Object;
        _utilisateurAuthenticationService = UtilisateurAuthenticationServiceMock.Object;
        _utilisateurCreationService = UtilisateurCreationServiceMock.Object;
        _passwordValidationService = PasswordValidationServiceMock.Object;
        _passwordHashingService = PasswordHashingServiceMock.Object;

        UnitOfWorkMock.Setup(u => u.FilmRepository).Returns(_filmRepository);
        UnitOfWorkMock.Setup(u => u.NoteFilmRepository).Returns(_noteFilmRepository);
        UnitOfWorkMock.Setup(u => u.ActeurRepository).Returns(_acteurRepository);
        UnitOfWorkMock.Setup(u => u.RealisateurRepository).Returns(_realisateurRepository);
        UnitOfWorkMock.Setup(u => u.CategorieFilmRepository).Returns(_categorieFilmRepository);
        UnitOfWorkMock.Setup(u => u.SalleRepository).Returns(_salleRepository);
        UnitOfWorkMock.Setup(u => u.ProjectionRepository).Returns(_projectionRepository);
        UnitOfWorkMock.Setup(u => u.UtilisateurRepository).Returns(_utilisateurRepository);
        UnitOfWorkFactoryMock.Setup(f => f.Create()).Returns(_unitOfWork);

        ServiceCollection serviceCollection = new();
        serviceCollection.AddSingleton<TService>();

        Dictionary<Type, object> serviceTypesToMock = new()
        {
            [typeof(IRepository<IActeur>)] = _acteurRepository,
            [typeof(IRepository<ICategorieFilm>)] = _categorieFilmRepository,
            [typeof(IRepository<IFilm>)] = _filmRepository,
            [typeof(IRepository<IProjection>)] = _projectionRepository,
            [typeof(IRepository<IRealisateur>)] = _realisateurRepository,
            [typeof(IRepository<ISalle>)] = _salleRepository,
            [typeof(IRepository<IUtilisateur>)] = _utilisateurRepository,
            [typeof(IUnitOfWork)] = _unitOfWork,
            [typeof(IUnitOfWorkFactory)] = _unitOfWorkFactory,
            [typeof(IUtilisateurAuthenticationService)] = _utilisateurAuthenticationService,
            [typeof(IUtilisateurCreationService)] = _utilisateurCreationService,
            [typeof(IPasswordValidationService)] = _passwordValidationService,
            [typeof(IPasswordHashingService)] = _passwordHashingService
        };

        foreach (KeyValuePair<Type, object> serviceType in serviceTypesToMock
                     .Where(serviceType => serviceCollection.All(sd => sd.ImplementationType != serviceType.Key)))
        {
            serviceCollection.AddSingleton(serviceType.Key, serviceType.Value);
        }

        ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

        SetUpExt();

        Service = serviceProvider.GetRequiredService<TService>();
    }

    protected virtual void SetUpExt() { }
}