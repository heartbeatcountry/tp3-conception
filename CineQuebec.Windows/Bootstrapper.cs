using CineQuebec.Windows.Views;

using Stylet;

using StyletIoC;

namespace CineQuebec.Windows;

public class Bootstrapper : Bootstrapper<RootViewModel>
{
    protected override void ConfigureIoC(IStyletIoCBuilder builder)
    {
        builder.AddModule(new MicrosoftDiModule());
        builder.Bind<NavigationController>().And<INavigationController>().To<NavigationController>().InSingletonScope();
    }

    protected override void OnLaunch()
    {
        NavigationController? navigationController = Container.Get<NavigationController>();
        navigationController.Delegate = RootViewModel;
        navigationController.NavigateTo<LoginViewModel>(null);
    }
}