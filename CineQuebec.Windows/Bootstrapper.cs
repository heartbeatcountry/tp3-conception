using System.Reflection;

using CineQuebec.Windows.Interfaces;
using CineQuebec.Windows.Interfaces.ViewModels.Screens;
using CineQuebec.Windows.ViewModels;

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
        builder.Bind<IGestionnaireExceptions>().ToAllImplementations().InSingletonScope();
        BindViewModelInterfaces(builder);
    }

    protected override void OnLaunch()
    {
        NavigationController? navigationController = Container.Get<NavigationController>();
        navigationController.Delegate = RootViewModel;
        navigationController.NavigateTo<ILoginViewModel>();
    }

    private static void BindViewModelInterfaces(IStyletIoCBuilder builder)
    {
        IEnumerable<Type> viewModelInterfaces = Assembly.GetExecutingAssembly()
            .GetTypes().Where(t => t.IsInterface && t.Name.EndsWith("ViewModel", StringComparison.InvariantCulture));

        foreach (Type viewModelInterface in viewModelInterfaces)
        {
            builder.Bind(viewModelInterface).ToAllImplementations();
        }
    }
}