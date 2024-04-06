using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stylet;

namespace CineQuebec.Windows.Views
{
    public class RootViewModel: Conductor<IScreen>, INavigationControllerDelegateFn
    {
        public void NavigateTo(IScreen screen)
        {
            this.ActivateItem(screen);
        }
    }
}
