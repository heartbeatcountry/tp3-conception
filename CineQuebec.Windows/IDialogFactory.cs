using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CineQuebec.Windows.Views;

namespace CineQuebec.Windows
{
    public interface IDialogFactory
    {
        DialogNomPrenomViewModel CreateDialogNomPrenom();
        DialogNomAffichageViewModel CreateDialogNomAffichage();
    }
}
