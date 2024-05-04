using CineQuebec.Windows;

using Microsoft.Extensions.DependencyInjection;

using Moq;

using Stylet;

using Tests.Common;

namespace Tests.Windows.ViewModels.Abstract;

public abstract class GenericViewModelTests<TViewModel> : GenericTestsWithServiceInjection<TViewModel>
    where TViewModel : class, IScreen
{
    protected Mock<Conductor<TViewModel>> ConductorMock { get; private set; } = null!;
    protected Mock<IDialogFactory> DialogFactoryMock { get; private set; } = null!;
    protected Mock<IGestionnaireExceptions> GestionnaireExceptionsMock { get; private set; } = null!;
    protected TViewModel ViewModel { get; private set; } = null!;

    [SetUp]
    public override void SetUp()
    {
        DialogFactoryMock = new Mock<IDialogFactory>();
        GestionnaireExceptionsMock = new Mock<IGestionnaireExceptions>();
        ConductorMock = new Mock<Conductor<TViewModel>>();

        base.SetUp();

        ViewModel = ServiceProvider.GetRequiredService<TViewModel>();
        ViewModel.Parent = ConductorMock.Object;
        ViewModel.ConductWith(ConductorMock.Object);
    }

    protected override IServiceCollection BuildServiceCollection()
    {
        return base.BuildServiceCollection()
            .AddSingleton(DialogFactoryMock.Object)
            .AddSingleton(GestionnaireExceptionsMock.Object);
    }
}