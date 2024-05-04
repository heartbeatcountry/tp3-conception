using Microsoft.Extensions.DependencyInjection;

using Tests.Common;

namespace Tests.Application.Services;

public abstract class GenericServiceTests<TService> : GenericTestsWithServiceInjection<TService> where TService : class
{
    protected TService Service { get; private set; } = null!;

    [SetUp]
    public new void SetUp()
    {
        base.SetUp();

        Service = ServiceProvider.GetRequiredService<TService>();
    }
}