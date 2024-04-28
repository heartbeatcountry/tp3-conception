using System.Linq.Expressions;
using System.Security;

using CineQuebec.Application.Services;
using CineQuebec.Domain.Interfaces.Entities.Utilisateur;

using Moq;

namespace Tests.Application.Services;

internal class UtilisateurCreationServiceTests : GenericServiceTests<UtilisateurCreationService>
{
    private const string PrenomValide = "Chantal";
    private const string PrenomInvalide = "L33tH4xx0r";
    private const string NomValide = "Gagnon";
    private const string NomInvalide = "G@ gn0n!";
    private const string CourrielValide = "chantal@gagnon.net";
    private const string CourrielInvalide = "@chantal";
    private const string MdpValide = "foo bar baz";
    private const string MdpInvalide = "soleil";

    [Test]
    public void CreerUtilisateur_WhenGivenInvalidCourriel_ShouldThrowAggregateExceptionContainingArgumentException()
    {
        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            Service.CreerUtilisateurAsync(PrenomValide, NomValide, CourrielInvalide, MdpValide));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("courriel n'est pas valide"));
    }

    [Test]
    public void CreerUtilisateur_WhenGivenEmptyCourriel_ShouldThrowAggregateExceptionContainingArgumentException()
    {
        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            Service.CreerUtilisateurAsync(PrenomValide, NomValide, string.Empty, MdpValide));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("courriel ne doit pas être vide"));
    }

    [Test]
    public void CreerUtilisateur_WhenGivenInvalidMdp_ShouldThrowAggregateExceptionContainingSecurityException()
    {
        // Arrange
        PasswordValidationServiceMock.Setup(pvs => pvs.ValiderMdpEstSecuritaire(It.IsAny<string>()))
            .Throws(new AggregateException(new SecurityException(
                "Le mot de passe doit contenir au moins 8 caractères")));

        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            Service.CreerUtilisateurAsync(PrenomValide, NomValide, CourrielValide, MdpInvalide));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<SecurityException>().With.Message.Contains("mot de passe doit contenir"));
    }

    [Test]
    public void CreerUtilisateur_WhenGivenValidArguments_ShouldCreateAndPersistNewUtilisateur()
    {
        // Arrange
        UtilisateurRepositoryMock.Setup(ur =>
            ur.ExisteAsync(It.IsAny<Expression<Func<IUtilisateur, bool>>>())).ReturnsAsync(false);
        PasswordHashingServiceMock.Setup(phs => phs.HacherMdp(It.IsAny<string>()))
            .Returns("hashedPassword");
        Guid mockUtilisateurId = Guid.NewGuid();
        IUtilisateur mockUtilisateur = Mock.Of<IUtilisateur>(u => u.Id == mockUtilisateurId);
        UtilisateurRepositoryMock.Setup(ur => ur.AjouterAsync(It.IsAny<IUtilisateur>()))
            .ReturnsAsync(mockUtilisateur);

        // Act
        Guid nouvUtilisateur = Service.CreerUtilisateurAsync(PrenomValide, NomValide, CourrielValide, MdpValide).Result;

        // Assert
        Assert.That(nouvUtilisateur, Is.EqualTo(mockUtilisateurId));
        UtilisateurRepositoryMock.Verify(ur => ur.AjouterAsync(It.IsAny<IUtilisateur>()), Times.Once);
        UnitOfWorkMock.Verify(uow => uow.SauvegarderAsync(It.IsAny<CancellationToken?>()), Times.Once);
    }

    [Test]
    public void CreerUtilisateur_WhenGivenEmptyPrenom_ShouldThrowAggregateExceptionContainingArgumentException()
    {
        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            Service.CreerUtilisateurAsync(string.Empty, NomValide, CourrielValide, MdpValide));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("prénom ne doit pas être vide"));
    }

    [Test]
    public void CreerUtilisateur_WhenGivenInvalidPrenom_ShouldThrowAggregateExceptionContainingArgumentException()
    {
        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            Service.CreerUtilisateurAsync(PrenomInvalide, NomValide, CourrielValide, MdpValide));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("prénom ne doit contenir"));
    }

    [Test]
    public void CreerUtilisateur_WhenGivenEmptyNom_ShouldThrowAggregateExceptionContainingArgumentException()
    {
        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            Service.CreerUtilisateurAsync(PrenomValide, string.Empty, CourrielValide, MdpValide));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("nom ne doit pas être vide"));
    }

    [Test]
    public void CreerUtilisateur_WhenGivenAlreadyUsedCourriel_ShouldThrowAggregateExceptionContainingArgumentException()
    {
        // Arrange
        UtilisateurRepositoryMock.Setup(ur =>
            ur.ExisteAsync(It.IsAny<Expression<Func<IUtilisateur, bool>>>())).ReturnsAsync(true);

        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            Service.CreerUtilisateurAsync(PrenomValide, NomValide, CourrielValide, MdpValide));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("courriel existe déjà"));
    }

    [Test]
    public void CreerUtilisateur_WhenGivenInvalidNom_ShouldThrowAggregateExceptionContainingArgumentException()
    {
        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            Service.CreerUtilisateurAsync(PrenomValide, NomInvalide, CourrielValide, MdpValide));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("nom ne doit contenir"));
    }
}