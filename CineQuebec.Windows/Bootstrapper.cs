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
        builder.Bind<Func<LoginViewModel>>().ToFactory<Func<LoginViewModel>>(c => () => c.Get<LoginViewModel>());
    }

    protected override void OnLaunch()
    {
        var navigationController = Container.Get<NavigationController>();
        navigationController.Delegate = RootViewModel;
        navigationController.NavigateToLogin();
    }
}