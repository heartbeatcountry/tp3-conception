using CineQuebec.Domain.Entities.Abstract;
using CineQuebec.Domain.Interfaces.Entities.Abstract;
using CineQuebec.Persistence.DbContext;
using CineQuebec.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;

namespace Tests.Persistence.Repositories;

internal class GenericRepositoryTests
{
    private TestDbContext _context = null!;

    private GenericRepository<EntiteTest, IEntiteTest> _repository = null!;

    [SetUp]
    public void Setup()
    {
        DbContextOptions<ApplicationDbContext> contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new TestDbContext(contextOptions);
        _repository = new GenericRepository<EntiteTest, IEntiteTest>(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context?.Dispose();
    }

    [Test]
    public async Task AjouterAsync_WhenGivenEntity_ShouldStoreEntity()
    {
        // Arrange
        EntiteTest entiteTest = new();
        entiteTest.SetId(Guid.NewGuid());

        // Act
        _ = await _repository.AjouterAsync(entiteTest);

        // Assert
        Assert.That(await _repository.ObtenirParIdAsync(entiteTest.Id), Is.EqualTo(entiteTest));
    }

    [Test]
    public async Task ExisteAsync_WhenEntityExists_ShouldReturnTrue()
    {
        // Arrange
        EntiteTest entiteTest = new();
        entiteTest.SetId(Guid.NewGuid());
        _ = await _repository.AjouterAsync(entiteTest);
        _ = await _context.SaveChangesAsync();

        // Act
        bool result = await _repository.ExisteAsync(f => f.Id == entiteTest.Id);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public async Task ObtenirParIdsAsync_WhenEntitiesExist_ShouldReturnEntities()
    {
        // Arrange
        EntiteTest entiteTest1 = new();
        entiteTest1.SetId(Guid.NewGuid());
        EntiteTest entiteTest2 = new();
        entiteTest2.SetId(Guid.NewGuid());
        _ = await _repository.AjouterAsync(entiteTest1);
        _ = await _repository.AjouterAsync(entiteTest2);
        _ = await _context.SaveChangesAsync();

        // Act
        IEnumerable<IEntiteTest> result = await _repository.ObtenirParIdsAsync([entiteTest1.Id, entiteTest2.Id]);

        // Assert
        Assert.That(result, Is.EquivalentTo(new[] { entiteTest1, entiteTest2 }));
    }

    [Test]
    public async Task ObtenirTousAsync_WhenEntitiesExist_ShouldReturnEntities()
    {
        // Arrange
        EntiteTest entiteTest1 = new();
        entiteTest1.SetId(Guid.NewGuid());
        EntiteTest entiteTest2 = new();
        entiteTest2.SetId(Guid.NewGuid());
        _ = await _repository.AjouterAsync(entiteTest1);
        _ = await _repository.AjouterAsync(entiteTest2);
        _ = await _context.SaveChangesAsync();

        // Act
        IEnumerable<IEntiteTest> result = await _repository.ObtenirTousAsync();

        // Assert
        Assert.That(result, Is.EquivalentTo(new[] { entiteTest1, entiteTest2 }));
    }

    [Test]
    public async Task Supprimer_WhenGivenEntity_ShouldRemoveEntity()
    {
        // Arrange
        EntiteTest entiteTest = new();
        entiteTest.SetId(Guid.NewGuid());
        _ = await _repository.AjouterAsync(entiteTest);
        _ = await _context.SaveChangesAsync();

        // Act
        _repository.Supprimer(entiteTest);
        _ = await _context.SaveChangesAsync();

        // Assert
        Assert.That(await _repository.ObtenirParIdAsync(entiteTest.Id), Is.Null);
    }

    [Test]
    public async Task ExisteAsync_WhenEntityDoesNotExist_ShouldReturnFalse()
    {
        // Act
        bool result = await _repository.ExisteAsync(f => f.Id == Guid.NewGuid());

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task Modifier_WhenEntityGivenEntity_ShouldUpdateEntity()
    {
        // Arrange
        EntiteTest entiteTest = new() { Prop = false };
        entiteTest.SetId(Guid.NewGuid());
        _ = await _repository.AjouterAsync(entiteTest);

        // Act
        entiteTest.Prop = true;
        _ = _repository.Modifier(entiteTest);

        // Assert
        Assert.That((await _repository.ObtenirParIdAsync(entiteTest.Id))!.Prop, Is.True);
    }

    [Test]
    public async Task ObtenirParIdAsync_WhenEntityExists_ShouldReturnEntity()
    {
        // Arrange
        EntiteTest entiteTest = new();
        await _repository.AjouterAsync(entiteTest);

        // Act
        IEntiteTest? result = await _repository.ObtenirParIdAsync(entiteTest.Id);

        // Assert
        Assert.That(result, Is.EqualTo(entiteTest));
    }

    private class TestDbContext(DbContextOptions<ApplicationDbContext> options) : ApplicationDbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<EntiteTest>();
        }
    }

    private interface IEntiteTest : IEntite
    {
        internal bool Prop { get; }
    }

    private class EntiteTest : Entite, IEntiteTest
    {
        public bool Prop { get; set; }
    }
}