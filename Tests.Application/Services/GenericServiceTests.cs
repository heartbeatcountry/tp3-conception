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
    private Mock<IUnitOfWorkFactory> _unitOfWorkFactoryMock = null!;
    protected TService Service { get; private set; } = null!;

    protected Mock<IUnitOfWork> UnitOfWorkMock { get; private set; } = null!;

    protected Mock<IUtilisateurAuthenticationService> UtilisateurAuthenticationServiceMock { get; private set; } =
        null!;

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
        CreateMocks();
        ConfigureUnitOfWork();
        IServiceProvider serviceProvider = SetupServiceInjection();

        SetUpExt();

        Service = serviceProvider.GetRequiredService<TService>();
    }

    protected virtual void SetUpExt()
    {
    }

    private void CreateMocks()
    {
        _unitOfWorkFactoryMock = new Mock<IUnitOfWorkFactory>();
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
        PasswordValidationServiceMock = new Mock<IPasswordValidationService>();
        PasswordHashingServiceMock = new Mock<IPasswordHashingService>();
    }

    private void ConfigureUnitOfWork()
    {
        UnitOfWorkMock.Setup(u => u.FilmRepository).Returns(FilmRepositoryMock.Object);
        UnitOfWorkMock.Setup(u => u.NoteFilmRepository).Returns(NoteFilmRepositoryMock.Object);
        UnitOfWorkMock.Setup(u => u.ActeurRepository).Returns(ActeurRepositoryMock.Object);
        UnitOfWorkMock.Setup(u => u.RealisateurRepository).Returns(RealisateurRepositoryMock.Object);
        UnitOfWorkMock.Setup(u => u.CategorieFilmRepository).Returns(CategorieFilmRepositoryMock.Object);
        UnitOfWorkMock.Setup(u => u.SalleRepository).Returns(SalleRepositoryMock.Object);
        UnitOfWorkMock.Setup(u => u.ProjectionRepository).Returns(ProjectionRepositoryMock.Object);
        UnitOfWorkMock.Setup(u => u.UtilisateurRepository).Returns(UtilisateurRepositoryMock.Object);
        _unitOfWorkFactoryMock.Setup(f => f.Create()).Returns(UnitOfWorkMock.Object);
    }

    private ServiceProvider SetupServiceInjection()
    {
        ServiceCollection serviceCollection = [];
        serviceCollection.AddSingleton<TService>();

        Dictionary<Type, object> serviceTypesToMock = new()
        {
            [typeof(IRepository<IActeur>)] = ActeurRepositoryMock.Object,
            [typeof(IRepository<ICategorieFilm>)] = CategorieFilmRepositoryMock.Object,
            [typeof(IRepository<IFilm>)] = FilmRepositoryMock.Object,
            [typeof(IRepository<IProjection>)] = ProjectionRepositoryMock.Object,
            [typeof(IRepository<IRealisateur>)] = RealisateurRepositoryMock.Object,
            [typeof(IRepository<ISalle>)] = SalleRepositoryMock.Object,
            [typeof(IRepository<IUtilisateur>)] = UtilisateurRepositoryMock.Object,
            [typeof(IRepository<INoteFilm>)] = NoteFilmRepositoryMock.Object,
            [typeof(IUnitOfWork)] = UnitOfWorkMock.Object,
            [typeof(IUnitOfWorkFactory)] = _unitOfWorkFactoryMock.Object,
            [typeof(IUtilisateurAuthenticationService)] = UtilisateurAuthenticationServiceMock.Object,
            [typeof(IPasswordValidationService)] = PasswordValidationServiceMock.Object,
            [typeof(IPasswordHashingService)] = PasswordHashingServiceMock.Object
        };

        foreach (KeyValuePair<Type, object> serviceType in serviceTypesToMock
                     .Where(serviceType => serviceCollection.All(sd => sd.ImplementationType != serviceType.Key)))
        {
            serviceCollection.AddSingleton(serviceType.Key, serviceType.Value);
        }

        return serviceCollection.BuildServiceProvider();
    }
}