using Microsoft.Extensions.DependencyInjection;

using Moq;

using Stylet;

using Tests.Common;

namespace Tests.Windows.Views.Abstract;

public abstract class GenericViewModelTests<TViewModel> : GenericTestsWithServiceInjection<TViewModel>
    where TViewModel : class, IScreen
{
    protected Mock<Conductor<TViewModel>> ConductorMock { get; private set; } = null!;
    protected TViewModel ViewModel { get; private set; } = null!;

    [SetUp]
    public new void SetUp()
    {
        base.SetUp();

        ConductorMock = new Mock<Conductor<TViewModel>>();
        Conductor<TViewModel> conductor = ConductorMock.Object;

        ViewModel = ServiceProvider.GetRequiredService<TViewModel>();
        ViewModel.Parent = conductor;
        ViewModel.ConductWith(conductor);
    }
}