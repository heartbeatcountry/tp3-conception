using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CineQuebec.Windows.Views;
using Stylet;

namespace CineQuebec.Windows
{
    public interface INavigationController
    {
        void NavigateToLogin ();
    }

    public interface INavigationControllerDelegateFn
    {
        void NavigateTo (IScreen screen);
    }

    public class NavigationController(Func<LoginViewModel> loginViewModelFactory)
        : INavigationController
    {
        public INavigationControllerDelegateFn? Delegate { get; set; }

        public void NavigateToLogin ()
        {
            this.Delegate?.NavigateTo(loginViewModelFactory());
        }
    }
}
