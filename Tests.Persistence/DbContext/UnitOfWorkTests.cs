using CineQuebec.Domain.Entities.Films;
using CineQuebec.Domain.Entities.Projections;
using CineQuebec.Persistence.DbContext;
using CineQuebec.Persistence.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace Tests.Persistence.DbContext;

internal class UnitOfWorkTests
{
    private UnitOfWork _unitOfWork = null!;

    [SetUp]
    public void Setup()
    {
        DbContextOptions<ApplicationDbContext> contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        IApplicationDbContextFactory contextFactory = new TestDbContextFactory(contextOptions);
        _unitOfWork = new UnitOfWork(contextFactory);
    }

    [TearDown]
    public void TearDown()
    {
        _unitOfWork?.Dispose();
    }

    [Test]
    public async Task SauvegarderAsync_WhenNoChanges_ShouldReturn0()
    {
        // Arrange
        // Act
        int result = await _unitOfWork.SauvegarderAsync();

        // Assert
        Assert.That(result, Is.Zero);
    }

    [Test]
    public async Task SauvegarderAsync_WhenTwoChanges_ShouldReturn2()
    {
        // Arrange
        _ = await _unitOfWork.ActeurRepository.AjouterAsync(new Acteur("A", "B"));
        _ = await _unitOfWork.ActeurRepository.AjouterAsync(new Acteur("B", "C"));

        // Act
        int result = await _unitOfWork.SauvegarderAsync();

        // Assert
        Assert.That(result, Is.EqualTo(2));
    }

    [Test]
    public void Dispose_WhenCalled_ShouldDisposeContext()
    {
        // Arrange
        // Act
        _unitOfWork.Dispose();

        // Assert
        Assert.Throws<ObjectDisposedException>(() => _ = _unitOfWork.SalleRepository);
    }

    [Test]
    public void Constructor_WhenInstantiating_ShouldInitializeRepositories()
    {
        // Assert
        Assert.That(_unitOfWork.SalleRepository, Is.Not.Null);
        Assert.That(_unitOfWork.ActeurRepository, Is.Not.Null);
        Assert.That(_unitOfWork.CategorieFilmRepository, Is.Not.Null);
        Assert.That(_unitOfWork.FilmRepository, Is.Not.Null);
        Assert.That(_unitOfWork.ProjectionRepository, Is.Not.Null);
        Assert.That(_unitOfWork.RealisateurRepository, Is.Not.Null);
    }

    private class TestDbContext(DbContextOptions<ApplicationDbContext> options) : ApplicationDbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Acteur>();
            builder.Entity<CategorieFilm>();
            builder.Entity<Film>();
            builder.Entity<Projection>();
            builder.Entity<Realisateur>();
            builder.Entity<Salle>();
        }
    }

    private class TestDbContextFactory(DbContextOptions<ApplicationDbContext> options) : IApplicationDbContextFactory
    {
        public ApplicationDbContext Create()
        {
            return new TestDbContext(options);
        }
    }
}