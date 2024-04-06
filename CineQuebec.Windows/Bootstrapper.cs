using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CineQuebec.Windows.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using CineQuebec.Application;
using CineQuebec.Persistence;

namespace CineQuebec.Windows
{
    public class Bootstrapper : Bootstrapper<RootViewModel>
    {
        protected override void ConfigureIoC (IStyletIoCBuilder builder)
        {
            builder.AddModule(new MicrosoftDiModule());
            builder.Bind<NavigationController>().And<INavigationController>().To<NavigationController>().InSingletonScope();
            builder.Bind<Func<LoginViewModel>>().ToFactory<Func<LoginViewModel>>(c => () => c.Get<LoginViewModel>());
        }

        protected override void OnLaunch ()
        {
            var navigationController = this.Container.Get<NavigationController>();
            navigationController.Delegate = this.RootViewModel;
            navigationController.NavigateToLogin();
        }
    }
}
