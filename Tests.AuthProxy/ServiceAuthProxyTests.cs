using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Claims;

using CineQuebec.Application.Interfaces.Services.Identity;
using CineQuebec.AuthProxy;
using CineQuebec.Domain.Entities.Utilisateurs;

using Microsoft.Extensions.DependencyInjection;

using Moq;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace Tests.AuthProxy;

internal interface ITestService
{
    public void TestMethod();
}

public class ServiceAuthProxyTests
{
    private Mock<IUtilisateurAuthenticationService> _authenticationServiceMock = null!;
    private ClaimsPrincipal? _claimsPrincipal;
    private Dictionary<Role, IEnumerable<string>> _methodMapping = null!;

    private ITestService? _proxy;
    private IServiceProvider _serviceProvider = null!;
    private ITestService _testService = null!;
    private Mock<ITestService> _testServiceMock = null!;
    private IUtilisateurAuthenticationService _utilisateurAuthenticationService = null!;

    private ITestService Proxy => _proxy ??= ServiceAuthProxy<ITestService>
        .CreerDispatchProxy(_testService, _serviceProvider, _methodMapping);

    [SetUp]
    public void Setup()
    {
        _authenticationServiceMock = new Mock<IUtilisateurAuthenticationService>();
        _authenticationServiceMock
            .Setup(a => a.ObtenirAutorisation())
            .Returns(() => _claimsPrincipal);
        _utilisateurAuthenticationService = _authenticationServiceMock.Object;

        _testServiceMock = new Mock<ITestService>();
        _testService = _testServiceMock.Object;

        _serviceProvider = new ServiceCollection()
            .AddSingleton(_utilisateurAuthenticationService)
            .BuildServiceProvider();

        _methodMapping = [];

        _claimsPrincipal = null;
        _proxy = null;
    }

    [TearDown]
    public void TearDown()
    {
        if (_serviceProvider is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }

    [Test]
    public void CreerDispatchProxy_ShouldReturnProxy()
    {
        // Assert
        Assert.That(Proxy, Is.InstanceOf<ServiceAuthProxy<ITestService>>());
    }

    [Test]
    public void Invoke_WhenMethodNotInMethodMapping_ShouldThrowSecurityException()
    {
        // Assert
        Assert.That(() => Proxy.TestMethod(),
            Throws.Exception.TypeOf<SecurityException>().With.Message
                .Contains("pas présente dans le mapping de rôles"));
    }

    [Test]
    public void Invoke_WhenRequiredRoleIsInvite_ShouldInvokeMethod()
    {
        // Arrange
        _methodMapping = new Dictionary<Role, IEnumerable<string>>
        {
            [Role.Invite] = [nameof(ITestService.TestMethod)]
        };

        // Act
        Proxy.TestMethod();

        // Assert
        _testServiceMock.Verify(s => s.TestMethod(), Times.Once);
    }

    [Test]
    public void Invoke_WhenClaimsPrincipalNotAuthentified_ShouldThrowSecurityException()
    {
        // Arrange
        _methodMapping = new Dictionary<Role, IEnumerable<string>>
        {
            [Role.Administrateur] = [nameof(ITestService.TestMethod)]
        };

        // Assert
        Assert.That(() => Proxy.TestMethod(),
            Throws.Exception.TypeOf<SecurityException>().With.Message
                .Contains("nécessite le rôle Administrateur"));
    }

    [Test]
    public void Invoke_WhenClaimsPrincipalNotAuthorized_ShouldThrowSecurityException()
    {
        // Arrange
        _claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(
        [
            new Claim(ClaimTypes.Role, Role.Utilisateur.ToString())
        ], "Basic"));

        _methodMapping = new Dictionary<Role, IEnumerable<string>>
        {
            [Role.Administrateur] = [nameof(ITestService.TestMethod)]
        };

        // Assert
        Assert.That(() => Proxy.TestMethod(),
            Throws.Exception.TypeOf<SecurityException>().With.Message
                .Contains("nécessite le rôle Administrateur"));
    }

    [Test]
    public void Invoke_WhenClaimsPrincipalIsAuthorized_ShouldInvokeMethod()
    {
        // Arrange
        _claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(
        [
            new Claim(ClaimTypes.Role, Role.Administrateur.ToString())
        ], "Basic"));

        _methodMapping = new Dictionary<Role, IEnumerable<string>>
        {
            [Role.Administrateur] = [nameof(ITestService.TestMethod)]
        };

        // Act
        Proxy.TestMethod();

        // Assert
        _testServiceMock.Verify(s => s.TestMethod(), Times.Once);
    }
}