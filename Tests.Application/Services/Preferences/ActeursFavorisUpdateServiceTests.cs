using CineQuebec.Application.Services.Preferences;
using CineQuebec.Domain.Interfaces.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Utilisateur;

using Moq;

namespace Tests.Application.Services.Preferences;

public class ActeursFavorisUpdateServiceTests : GenericServiceTests<ActeursFavorisUpdateService>
{
    private readonly IActeur _acteur1 = Mock.Of<IActeur>(a => a.Id == Guid.NewGuid());
    private readonly IActeur _acteur2 = Mock.Of<IActeur>(a => a.Id == Guid.NewGuid());
    private readonly IActeur _acteur3 = Mock.Of<IActeur>(a => a.Id == Guid.NewGuid());
    private readonly Guid _idUtilisateur = Guid.NewGuid();
    private Mock<IUtilisateur> _utilisateurMock = null!;

    [SetUp]
    public new void SetUp()
    {
        base.SetUp();

        _utilisateurMock = new Mock<IUtilisateur>();
        _utilisateurMock.SetupGet(u => u.Id).Returns(_idUtilisateur);
        _utilisateurMock.SetupGet(u => u.ActeursFavorisParId).Returns([
            _acteur1.Id, _acteur2.Id
        ]);

        UtilisateurAuthenticationServiceMock.Setup(uas => uas.ObtenirIdUtilisateurConnecte())
            .Returns(_idUtilisateur);

        UtilisateurRepositoryMock.Setup(ur => ur.ObtenirParIdAsync(_idUtilisateur))
            .ReturnsAsync(_utilisateurMock.Object);
    }

    [Test]
    public void AjouterActeurFavori_WhenActeurExists_ShouldAddActeurToUser()
    {
        // Arrange
        ActeurRepositoryMock.Setup(ar => ar.ExisteAsync(_acteur3.Id))
            .ReturnsAsync(true);

        // Act
        Service.AjouterActeurFavori(_acteur3.Id).Wait();

        // Assert
        _utilisateurMock.Verify(u => u.AddActeursFavoris(new[] { _acteur3.Id }), Times.Once);
        UtilisateurRepositoryMock.Verify(ur => ur.Modifier(_utilisateurMock.Object), Times.Once);
        UnitOfWorkMock.Verify(uow => uow.SauvegarderAsync(It.IsAny<CancellationToken?>()), Times.Once);
    }

    [Test]
    public void AjouterActeurFavori_WhenActeurDoesNotExist_ShouldThrowAggregateExceptionContainingArgumentException()
    {
        // Arrange
        ActeurRepositoryMock.Setup(ar => ar.ExisteAsync(_acteur3.Id))
            .ReturnsAsync(false);

        // Act
        AggregateException exception =
            Assert.ThrowsAsync<AggregateException>(() => Service.AjouterActeurFavori(_acteur3.Id))!;

        // Assert
        Assert.That(exception?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("n'existe pas"));
    }

    [Test]
    public void AjouterActeurFavori_WhenActeurAlreadyAdded_ShouldNotAddActeur()
    {
        // Act
        Service.AjouterActeurFavori(_acteur2.Id).Wait();

        // Assert
        _utilisateurMock.Verify(u => u.AddActeursFavoris(It.IsAny<IEnumerable<Guid>>()), Times.Never);
        UtilisateurRepositoryMock.Verify(ur => ur.Modifier(_utilisateurMock.Object), Times.Never);
        UnitOfWorkMock.Verify(uow => uow.SauvegarderAsync(It.IsAny<CancellationToken?>()), Times.Never);
    }

    [Test]
    public void RetirerActeurFavori_WhenActeurExists_ShouldRemoveActeurFromUser()
    {
        // Act
        Service.RetirerActeurFavori(_acteur2.Id).Wait();

        // Assert
        _utilisateurMock.Verify(u => u.RemoveActeursFavoris(new[] { _acteur2.Id }), Times.Once);
        UtilisateurRepositoryMock.Verify(ur => ur.Modifier(_utilisateurMock.Object), Times.Once);
        UnitOfWorkMock.Verify(uow => uow.SauvegarderAsync(It.IsAny<CancellationToken?>()), Times.Once);
    }

    [Test]
    public void RetirerActeurFavori_WhenActeurNotAdded_ShouldNotAttemptToRemoveActeur()
    {
        // Act
        Service.RetirerActeurFavori(_acteur3.Id).Wait();

        // Assert
        _utilisateurMock.Verify(u => u.RemoveActeursFavoris(It.IsAny<IEnumerable<Guid>>()), Times.Never);
        UtilisateurRepositoryMock.Verify(ur => ur.Modifier(_utilisateurMock.Object), Times.Never);
        UnitOfWorkMock.Verify(uow => uow.SauvegarderAsync(It.IsAny<CancellationToken?>()), Times.Never);
    }
}