using CineQuebec.Windows.Interfaces.ViewModels.Dialogs;
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
        builder.Bind<NavigationController>().And<INavigationController>().To<NavigationController>().InSingletonScope();
        builder.Bind<IGestionnaireExceptions>().ToAllImplementations().InSingletonScope();
        BindDialogs(builder);
    }

    protected override void OnLaunch()
    {
        NavigationController? navigationController = Container.Get<NavigationController>();
        navigationController.Delegate = RootViewModel;
        navigationController.NavigateTo<LoginViewModel>();
    }

    private static void BindDialogs(IStyletIoCBuilder builder)
    {
        builder.Bind<IDialogFactory>().ToAbstractFactory();
        builder.Bind<IDialogInscriptionUtilisateurViewModel>().ToAllImplementations();
    }
}