using CineQuebec.Application.Services.Preferences;
using CineQuebec.Domain.Interfaces.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Utilisateur;

using Moq;

namespace Tests.Application.Services.Preferences;

public class CategoriesPrefereesUpdateServiceTests : GenericServiceTests<CategoriesPrefereesUpdateService>
{
    private readonly ICategorieFilm _categorie1 = Mock.Of<ICategorieFilm>(a => a.Id == Guid.NewGuid());
    private readonly ICategorieFilm _categorie2 = Mock.Of<ICategorieFilm>(a => a.Id == Guid.NewGuid());
    private readonly ICategorieFilm _categorie3 = Mock.Of<ICategorieFilm>(a => a.Id == Guid.NewGuid());
    private readonly Guid _idUtilisateur = Guid.NewGuid();
    private Mock<IUtilisateur> _utilisateurMock = null!;

    [SetUp]
    public new void SetUp()
    {
        base.SetUp();

        _utilisateurMock = new Mock<IUtilisateur>();
        _utilisateurMock.SetupGet(u => u.Id).Returns(_idUtilisateur);
        _utilisateurMock.SetupGet(u => u.CategoriesPrefereesParId).Returns([
            _categorie1.Id, _categorie2.Id
        ]);

        UtilisateurAuthenticationServiceMock.Setup(uas => uas.ObtenirIdUtilisateurConnecte())
            .Returns(_idUtilisateur);

        UtilisateurRepositoryMock.Setup(ur => ur.ObtenirParIdAsync(_idUtilisateur))
            .ReturnsAsync(_utilisateurMock.Object);
    }

    [Test]
    public void AjouterCategoriePreferee_WhenCategorieExists_ShouldAddCategorieToUser()
    {
        // Arrange
        CategorieFilmRepositoryMock.Setup(ar => ar.ExisteAsync(_categorie3.Id))
            .ReturnsAsync(true);

        // Act
        Service.AjouterCategoriePreferee(_categorie3.Id).Wait();

        // Assert
        _utilisateurMock.Verify(u => u.AddCategoriesPreferees(new[] { _categorie3.Id }), Times.Once);
        UtilisateurRepositoryMock.Verify(ur => ur.Modifier(_utilisateurMock.Object), Times.Once);
        UnitOfWorkMock.Verify(uow => uow.SauvegarderAsync(It.IsAny<CancellationToken?>()), Times.Once);
    }

    [Test]
    public void
        AjouterCategoriePreferee_WhenCategorieDoesNotExist_ShouldThrowAggregateExceptionContainingArgumentException()
    {
        // Arrange
        CategorieFilmRepositoryMock.Setup(ar => ar.ExisteAsync(_categorie3.Id))
            .ReturnsAsync(false);

        // Act
        AggregateException exception =
            Assert.ThrowsAsync<AggregateException>(() => Service.AjouterCategoriePreferee(_categorie3.Id))!;

        // Assert
        Assert.That(exception?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("n'existe pas"));
    }

    [Test]
    public void AjouterCategoriePreferee_WhenCategorieAlreadyAdded_ShouldNotAddCategorie()
    {
        // Act
        Service.AjouterCategoriePreferee(_categorie2.Id).Wait();

        // Assert
        _utilisateurMock.Verify(u => u.AddCategoriesPreferees(It.IsAny<IEnumerable<Guid>>()), Times.Never);
        UtilisateurRepositoryMock.Verify(ur => ur.Modifier(_utilisateurMock.Object), Times.Never);
        UnitOfWorkMock.Verify(uow => uow.SauvegarderAsync(It.IsAny<CancellationToken?>()), Times.Never);
    }

    [Test]
    public void RetirerCategoriePreferee_WhenCategorieExists_ShouldRemoveCategorieFromUser()
    {
        // Act
        Service.RetirerCategoriePreferee(_categorie2.Id).Wait();

        // Assert
        _utilisateurMock.Verify(u => u.RemoveCategoriesPreferees(new[] { _categorie2.Id }), Times.Once);
        UtilisateurRepositoryMock.Verify(ur => ur.Modifier(_utilisateurMock.Object), Times.Once);
        UnitOfWorkMock.Verify(uow => uow.SauvegarderAsync(It.IsAny<CancellationToken?>()), Times.Once);
    }

    [Test]
    public void RetirerCategoriePreferee_WhenCategorieNotAdded_ShouldNotAttemptToRemoveCategorie()
    {
        // Act
        Service.RetirerCategoriePreferee(_categorie3.Id).Wait();

        // Assert
        _utilisateurMock.Verify(u => u.RemoveCategoriesPreferees(It.IsAny<IEnumerable<Guid>>()), Times.Never);
        UtilisateurRepositoryMock.Verify(ur => ur.Modifier(_utilisateurMock.Object), Times.Never);
        UnitOfWorkMock.Verify(uow => uow.SauvegarderAsync(It.IsAny<CancellationToken?>()), Times.Never);
    }
}