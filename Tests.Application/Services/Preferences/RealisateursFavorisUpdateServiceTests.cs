using CineQuebec.Application.Services.Preferences;
using CineQuebec.Domain.Interfaces.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Utilisateur;

using Moq;

namespace Tests.Application.Services.Preferences;

public class RealisateursFavorisUpdateServiceTests : GenericServiceTests<RealisateursFavorisUpdateService>
{
    private readonly Guid _idUtilisateur = Guid.NewGuid();
    private readonly IRealisateur _realisateur1 = Mock.Of<IRealisateur>(a => a.Id == Guid.NewGuid());
    private readonly IRealisateur _realisateur2 = Mock.Of<IRealisateur>(a => a.Id == Guid.NewGuid());
    private readonly IRealisateur _realisateur3 = Mock.Of<IRealisateur>(a => a.Id == Guid.NewGuid());
    private Mock<IUtilisateur> _utilisateurMock = null!;

    [SetUp]
    public new void SetUp()
    {
        base.SetUp();

        _utilisateurMock = new Mock<IUtilisateur>();
        _utilisateurMock.SetupGet(u => u.Id).Returns(_idUtilisateur);
        _utilisateurMock.SetupGet(u => u.RealisateursFavorisParId).Returns([
            _realisateur1.Id, _realisateur2.Id
        ]);

        UtilisateurAuthenticationServiceMock.Setup(uas => uas.ObtenirIdUtilisateurConnecte())
            .Returns(_idUtilisateur);

        UtilisateurRepositoryMock.Setup(ur => ur.ObtenirParIdAsync(_idUtilisateur))
            .ReturnsAsync(_utilisateurMock.Object);
    }

    [Test]
    public void AjouterRealisateurFavori_WhenRealisateurExists_ShouldAddRealisateurToUser()
    {
        // Arrange
        RealisateurRepositoryMock.Setup(ar => ar.ExisteAsync(_realisateur3.Id))
            .ReturnsAsync(true);

        // Act
        Service.AjouterRealisateurFavori(_realisateur3.Id).Wait();

        // Assert
        _utilisateurMock.Verify(u => u.AddRealisateursFavoris(new[] { _realisateur3.Id }), Times.Once);
        UtilisateurRepositoryMock.Verify(ur => ur.Modifier(_utilisateurMock.Object), Times.Once);
        UnitOfWorkMock.Verify(uow => uow.SauvegarderAsync(It.IsAny<CancellationToken?>()), Times.Once);
    }

    [Test]
    public void
        AjouterRealisateurFavori_WhenRealisateurDoesNotExist_ShouldThrowAggregateExceptionContainingArgumentException()
    {
        // Arrange
        RealisateurRepositoryMock.Setup(ar => ar.ExisteAsync(_realisateur3.Id))
            .ReturnsAsync(false);

        // Act
        AggregateException exception =
            Assert.ThrowsAsync<AggregateException>(() => Service.AjouterRealisateurFavori(_realisateur3.Id))!;

        // Assert
        Assert.That(exception?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("n'existe pas"));
    }

    [Test]
    public void AjouterRealisateurFavori_WhenRealisateurAlreadyAdded_ShouldNotAddRealisateur()
    {
        // Act
        Service.AjouterRealisateurFavori(_realisateur2.Id).Wait();

        // Assert
        _utilisateurMock.Verify(u => u.AddRealisateursFavoris(It.IsAny<IEnumerable<Guid>>()), Times.Never);
        UtilisateurRepositoryMock.Verify(ur => ur.Modifier(_utilisateurMock.Object), Times.Never);
        UnitOfWorkMock.Verify(uow => uow.SauvegarderAsync(It.IsAny<CancellationToken?>()), Times.Never);
    }

    [Test]
    public void RetirerRealisateurFavori_WhenRealisateurExists_ShouldRemoveRealisateurFromUser()
    {
        // Act
        Service.RetirerRealisateurFavori(_realisateur2.Id).Wait();

        // Assert
        _utilisateurMock.Verify(u => u.RemoveRealisateursFavoris(new[] { _realisateur2.Id }), Times.Once);
        UtilisateurRepositoryMock.Verify(ur => ur.Modifier(_utilisateurMock.Object), Times.Once);
        UnitOfWorkMock.Verify(uow => uow.SauvegarderAsync(It.IsAny<CancellationToken?>()), Times.Once);
    }

    [Test]
    public void RetirerRealisateurFavori_WhenRealisateurNotAdded_ShouldNotAttemptToRemoveRealisateur()
    {
        // Act
        Service.RetirerRealisateurFavori(_realisateur3.Id).Wait();

        // Assert
        _utilisateurMock.Verify(u => u.RemoveRealisateursFavoris(It.IsAny<IEnumerable<Guid>>()), Times.Never);
        UtilisateurRepositoryMock.Verify(ur => ur.Modifier(_utilisateurMock.Object), Times.Never);
        UnitOfWorkMock.Verify(uow => uow.SauvegarderAsync(It.IsAny<CancellationToken?>()), Times.Never);
    }
}