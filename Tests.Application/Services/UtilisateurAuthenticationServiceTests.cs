using System.Linq.Expressions;
using System.Security.Claims;

using CineQuebec.Application.Services;
using CineQuebec.Domain.Entities.Utilisateurs;
using CineQuebec.Domain.Interfaces.Entities.Utilisateur;

using Moq;

namespace Tests.Application.Services;

internal class UtilisateurAuthenticationServiceTests : GenericServiceTests<UtilisateurAuthenticationService>
{
    private const string CourrielValide = "courriel@valide.com";
    private const string MdpValide = "mdp";
    private const string CourrielIntrouvable = "courriel@introuvable.com";
    private const string MdpInvalide = "invalide";
    private const string MdpHache = "abcd";

    [Test]
    public void AuthentifierThreadAsync_WhenGivenEmptyMdp_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        ArgumentNullException? exception =
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                Service.AuthentifierThreadAsync(CourrielValide, String.Empty));
        Assert.That(exception, Is.InstanceOf<ArgumentNullException>());
    }

    [Test]
    public void AuthentifierThreadAsync_WhenGivenEmptyNomUsager_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        ArgumentNullException? exception =
            Assert.ThrowsAsync<ArgumentNullException>(() => Service.AuthentifierThreadAsync(String.Empty, MdpValide));
        Assert.That(exception, Is.InstanceOf<ArgumentNullException>());
    }

    [Test]
    public void AuthentifierThreadAsync_WhenGivenInvalidCourriel_ShouldThrowKeyNotFoundException()
    {
        // Act & Assert
        KeyNotFoundException? exception =
            Assert.ThrowsAsync<KeyNotFoundException>(() =>
                Service.AuthentifierThreadAsync(CourrielIntrouvable, MdpValide));
        Assert.That(exception, Is.InstanceOf<KeyNotFoundException>());
    }

    [Test]
    public void AuthentifierThreadAsync_WhenGivenInvalidMdp_ShouldThrowKeyNotFoundException()
    {
        // Act & Assert
        KeyNotFoundException? exception =
            Assert.ThrowsAsync<KeyNotFoundException>(() =>
                Service.AuthentifierThreadAsync(CourrielValide, MdpInvalide));
        Assert.That(exception, Is.InstanceOf<KeyNotFoundException>());
    }

    [Test]
    public async Task AuthentifierThreadAsync_WhenGivenValidCredentials_ShouldInitializeClaimsPrincipal()
    {
        // Arrange
        Role[] roles = [Role.Utilisateur];
        IUtilisateur utilisateurMock = Mock.Of<IUtilisateur>(u =>
            u.Courriel == CourrielValide && u.HashMotDePasse == MdpHache && u.Roles == roles);
        UtilisateurRepositoryMock.Setup(ur => ur.ObtenirAsync(It.IsAny<Expression<Func<IUtilisateur, bool>>>()))
            .ReturnsAsync(utilisateurMock);
        PasswordHashingServiceMock.Setup(phs => phs.ValiderMdp(MdpValide, MdpHache)).Returns(true);
        PasswordHashingServiceMock.Setup(phs => phs.DoitEtreRehache(MdpHache)).Returns(false);

        // Act
        await Service.AuthentifierThreadAsync(CourrielValide, MdpValide);

        // Assert
        ClaimsPrincipal? claimsPrincipal = Service.ObtenirAutorisation();
        Assert.Multiple(() =>
        {
            Assert.That(claimsPrincipal, Is.Not.Null);
            Assert.That(claimsPrincipal?.Identity?.IsAuthenticated, Is.True);
            Assert.That(claimsPrincipal?.Identity?.Name, Is.EqualTo(CourrielValide));
            Assert.That(claimsPrincipal?.IsInRole(Role.Utilisateur.ToString()), Is.True);
        });
    }

    [Test]
    public void AuthentifierThreadAsync_WhenUtilisateurNotFound_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        UtilisateurRepositoryMock.Setup(ur => ur.ObtenirAsync(It.IsAny<Expression<Func<IUtilisateur, bool>>>()))
            .ReturnsAsync((IUtilisateur?)null);

        // Act & Assert
        KeyNotFoundException? exception =
            Assert.ThrowsAsync<KeyNotFoundException>(() => Service.AuthentifierThreadAsync(CourrielValide, MdpValide));
        Assert.That(exception, Is.InstanceOf<KeyNotFoundException>());
    }

    [Test]
    public void AuthentifierThreadAsync_WhenPasswordIsWrong_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        Role[] roles = [Role.Utilisateur];
        IUtilisateur utilisateurMock = Mock.Of<IUtilisateur>(u =>
            u.Courriel == CourrielValide && u.HashMotDePasse == MdpHache && u.Roles == roles);
        UtilisateurRepositoryMock.Setup(ur => ur.ObtenirAsync(It.IsAny<Expression<Func<IUtilisateur, bool>>>()))
            .ReturnsAsync(utilisateurMock);
        PasswordHashingServiceMock.Setup(phs => phs.ValiderMdp(MdpValide, MdpHache)).Returns(false);

        // Act & Assert
        KeyNotFoundException? exception =
            Assert.ThrowsAsync<KeyNotFoundException>(() => Service.AuthentifierThreadAsync(CourrielValide, MdpValide));
        Assert.That(exception, Is.InstanceOf<KeyNotFoundException>());
    }

    [Test]
    public async Task AuthentifierThreadAsync_WhenPasswordNeedsRehashing_ShouldRehashPassword()
    {
        // Arrange
        Role[] roles = [Role.Utilisateur];
        IUtilisateur utilisateurMock = Mock.Of<IUtilisateur>(u =>
            u.Courriel == CourrielValide && u.HashMotDePasse == MdpHache && u.Roles == roles);
        UtilisateurRepositoryMock.Setup(ur => ur.ObtenirAsync(It.IsAny<Expression<Func<IUtilisateur, bool>>>()))
            .ReturnsAsync(utilisateurMock);
        PasswordHashingServiceMock.Setup(phs => phs.ValiderMdp(MdpValide, MdpHache)).Returns(true);
        PasswordHashingServiceMock.Setup(phs => phs.DoitEtreRehache(MdpHache)).Returns(true);
        PasswordHashingServiceMock.Setup(phs => phs.HacherMdp(It.IsAny<string>())).Returns(MdpHache);

        // Act
        await Service.AuthentifierThreadAsync(CourrielValide, MdpValide);

        // Assert
        UtilisateurRepositoryMock.Verify(ur => ur.Modifier(It.IsAny<IUtilisateur>()), Times.Once);
        UnitOfWorkMock.Verify(u => u.SauvegarderAsync(It.IsAny<CancellationToken?>()), Times.Once);
        PasswordHashingServiceMock.Verify(phs => phs.HacherMdp(MdpValide), Times.Once);
    }

    [Test]
    public void DeauthentifierThread_WhenCalled_ShouldSetCurrentPrincipalToNull()
    {
        // Arrange
        Thread.CurrentPrincipal = new ClaimsPrincipal();

        // Act
        Service.DeauthentifierThread();

        // Assert
        ClaimsPrincipal? claimsPrincipal = Service.ObtenirAutorisation();
        Assert.That(claimsPrincipal, Is.Null);
    }
}