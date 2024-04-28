﻿using CineQuebec.Application.Interfaces.Services;
using CineQuebec.AuthProxy;

using Microsoft.Extensions.DependencyInjection;

using Moq;

namespace Tests.AuthProxy;

public class ServiceCollectionExtensionsTests
{
    private IServiceCollection _services = null!;

    [SetUp]
    public void Setup()
    {
        _services = new ServiceCollection();
        _services.AddSingleton<ITestService, TestService>();
    }

    [Test]
    public void AjouterProxyAuthPourService_ShouldReplaceServiceDescriptor()
    {
        // Act
        _services.AjouterProxyAuthPourService<ITestService>([]);

        // Assert
        int serviceCount = _services.Count(s => s.ServiceType == typeof(ITestService));
        Assert.That(serviceCount, Is.EqualTo(1));
    }

    [Test]
    public void AjouterProxyAuthPourService_ShouldReturnSameServiceCollection()
    {
        // Act
        IServiceCollection serviceCollection = _services.AjouterProxyAuthPourService<ITestService>([]);

        // Assert
        Assert.That(serviceCollection, Is.EqualTo(_services));
    }

    [Test]
    public void AjouterProxyAuthPourService_ShouldWrapServiceInServiceAuthProxy()
    {
        // Arrange
        IUtilisateurAuthenticationService utilisateurAuthenticationService =
            Mock.Of<IUtilisateurAuthenticationService>();
        _services.AddSingleton(utilisateurAuthenticationService);

        // Act
        _services.AjouterProxyAuthPourService<ITestService>([]);

        // Assert
        ITestService service = _services.BuildServiceProvider().GetRequiredService<ITestService>();
        Assert.That(service, Is.InstanceOf<ServiceAuthProxy<ITestService>>());
    }

    private interface ITestService
    {
    }

    private class TestService : ITestService
    {
    }
}