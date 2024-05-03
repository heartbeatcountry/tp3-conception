using CineQuebec.Application;
using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services.Films;
using CineQuebec.Application.Interfaces.Services.Projections;
using CineQuebec.AuthProxy;

using Microsoft.Extensions.DependencyInjection;

using Moq;

namespace Tests.AuthProxy;

public class ConfigureServicesTests
{
    [Test]
    public void ServicesAreRegisteredCorrectly()
    {
        // Arrange
        ServiceCollection services = [];
        services
            .AddSingleton(Mock.Of<IUnitOfWorkFactory>())
            .AddApplicationServices()
            .WrapServicesWithAuthProxy();

        // Act
        ServiceProvider provider = services.BuildServiceProvider();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.DoesNotThrow(() => provider.GetRequiredService<IFilmQueryService>());
            Assert.That(provider.GetRequiredService<IFilmQueryService>(),
                Is.InstanceOf<ServiceAuthProxy<IFilmQueryService>>());
            Assert.DoesNotThrow(() => provider.GetRequiredService<IBilletQueryService>());
            Assert.That(provider.GetRequiredService<IBilletQueryService>(),
                Is.InstanceOf<ServiceAuthProxy<IBilletQueryService>>());
            Assert.DoesNotThrow(() => provider.GetRequiredService<IFilmCreationService>());
            Assert.That(provider.GetRequiredService<IFilmCreationService>(),
                Is.InstanceOf<ServiceAuthProxy<IFilmCreationService>>());
            Assert.DoesNotThrow(() => provider.GetRequiredService<IFilmDeletionService>());
            Assert.That(provider.GetRequiredService<IFilmDeletionService>(),
                Is.InstanceOf<ServiceAuthProxy<IFilmDeletionService>>());
            Assert.DoesNotThrow(() => provider.GetRequiredService<IFilmUpdateService>());
            Assert.That(provider.GetRequiredService<IFilmUpdateService>(),
                Is.InstanceOf<ServiceAuthProxy<IFilmUpdateService>>());
            Assert.DoesNotThrow(() => provider.GetRequiredService<IActeurCreationService>());
            Assert.That(provider.GetRequiredService<IActeurCreationService>(),
                Is.InstanceOf<ServiceAuthProxy<IActeurCreationService>>());
            Assert.DoesNotThrow(() => provider.GetRequiredService<IActeurQueryService>());
            Assert.That(provider.GetRequiredService<IActeurQueryService>(),
                Is.InstanceOf<ServiceAuthProxy<IActeurQueryService>>());
            Assert.DoesNotThrow(() => provider.GetRequiredService<IRealisateurCreationService>());
            Assert.That(provider.GetRequiredService<IRealisateurCreationService>(),
                Is.InstanceOf<ServiceAuthProxy<IRealisateurCreationService>>());
            Assert.DoesNotThrow(() => provider.GetRequiredService<IRealisateurQueryService>());
            Assert.That(provider.GetRequiredService<IRealisateurQueryService>(),
                Is.InstanceOf<ServiceAuthProxy<IRealisateurQueryService>>());
            Assert.DoesNotThrow(() => provider.GetRequiredService<ICategorieFilmCreationService>());
            Assert.That(provider.GetRequiredService<ICategorieFilmCreationService>(),
                Is.InstanceOf<ServiceAuthProxy<ICategorieFilmCreationService>>());
            Assert.DoesNotThrow(() => provider.GetRequiredService<ICategorieFilmQueryService>());
            Assert.That(provider.GetRequiredService<ICategorieFilmQueryService>(),
                Is.InstanceOf<ServiceAuthProxy<ICategorieFilmQueryService>>());
            Assert.DoesNotThrow(() => provider.GetRequiredService<IProjectionCreationService>());
            Assert.That(provider.GetRequiredService<IProjectionCreationService>(),
                Is.InstanceOf<ServiceAuthProxy<IProjectionCreationService>>());
            Assert.DoesNotThrow(() => provider.GetRequiredService<IProjectionQueryService>());
            Assert.That(provider.GetRequiredService<IProjectionQueryService>(),
                Is.InstanceOf<ServiceAuthProxy<IProjectionQueryService>>());
            Assert.DoesNotThrow(() => provider.GetRequiredService<IProjectionDeletionService>());
            Assert.That(provider.GetRequiredService<IProjectionDeletionService>(),
                Is.InstanceOf<ServiceAuthProxy<IProjectionDeletionService>>());
            Assert.DoesNotThrow(() => provider.GetRequiredService<ISalleCreationService>());
            Assert.That(provider.GetRequiredService<ISalleCreationService>(),
                Is.InstanceOf<ServiceAuthProxy<ISalleCreationService>>());
            Assert.DoesNotThrow(() => provider.GetRequiredService<ISalleQueryService>());
            Assert.That(provider.GetRequiredService<ISalleQueryService>(),
                Is.InstanceOf<ServiceAuthProxy<ISalleQueryService>>());
        });
    }
}