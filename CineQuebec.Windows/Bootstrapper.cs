using CineQuebec.Windows.ViewModels;
using CineQuebec.Windows.ViewModels.Screens;

using Stylet;

using StyletIoC;

namespace CineQuebec.Windows;

public class Bootstrapper : Bootstrapper<RootViewModel>
{
    protected override void ConfigureIoC(IStyletIoCBuilder builder)
    {
        builder.AddModule(new MicrosoftDiModule());
        builder.Bind<IDialogFactory>().ToAbstractFactory();
        builder.Bind<NavigationController>().And<INavigationController>().To<NavigationController>().InSingletonScope();
        builder.Bind<IGestionnaireExceptions>().To<GestionnaireExceptions>().InSingletonScope();
    }

    protected override void OnLaunch()
    {
        NavigationController? navigationController = Container.Get<NavigationController>();
        navigationController.Delegate = RootViewModel;
        navigationController.NavigateTo<LoginViewModel>();
    }
}