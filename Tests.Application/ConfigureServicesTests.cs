using CineQuebec.Application;
using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services;

using Microsoft.Extensions.DependencyInjection;

using Moq;

namespace Tests.Application;

public class ConfigureServicesTests
{
    [Test]
    public void ServicesAreRegisteredCorrectly()
    {
        // Arrange
        ServiceCollection services = [];
        services.AddApplicationServices();
        services.AddSingleton(Mock.Of<IUnitOfWorkFactory>());

        // Act
        ServiceProvider provider = services.BuildServiceProvider();

        // Assert
        Assert.DoesNotThrow(() => provider.GetRequiredService<IFilmQueryService>());
        Assert.DoesNotThrow(() => provider.GetRequiredService<IFilmCreationService>());
        Assert.DoesNotThrow(() => provider.GetRequiredService<IFilmDeletionService>());
        Assert.DoesNotThrow(() => provider.GetRequiredService<IFilmUpdateService>());
        Assert.DoesNotThrow(() => provider.GetRequiredService<IActeurCreationService>());
        Assert.DoesNotThrow(() => provider.GetRequiredService<IActeurQueryService>());
        Assert.DoesNotThrow(() => provider.GetRequiredService<IRealisateurCreationService>());
        Assert.DoesNotThrow(() => provider.GetRequiredService<IRealisateurQueryService>());
        Assert.DoesNotThrow(() => provider.GetRequiredService<ICategorieFilmCreationService>());
        Assert.DoesNotThrow(() => provider.GetRequiredService<ICategorieFilmQueryService>());
        Assert.DoesNotThrow(() => provider.GetRequiredService<IProjectionCreationService>());
        Assert.DoesNotThrow(() => provider.GetRequiredService<IProjectionQueryService>());
        Assert.DoesNotThrow(() => provider.GetRequiredService<IProjectionDeletionService>());
        Assert.DoesNotThrow(() => provider.GetRequiredService<ISalleCreationService>());
        Assert.DoesNotThrow(() => provider.GetRequiredService<ISalleQueryService>());
        Assert.DoesNotThrow(() => provider.GetRequiredService<IUtilisateurAuthenticationService>());
        Assert.DoesNotThrow(() => provider.GetRequiredService<IUtilisateurCreationService>());
        Assert.DoesNotThrow(() => provider.GetRequiredService<IPasswordHashingService>());
        Assert.DoesNotThrow(() => provider.GetRequiredService<IPasswordValidationService>());
    }
}