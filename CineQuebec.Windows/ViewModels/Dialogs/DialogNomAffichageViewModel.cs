using CineQuebec.Windows.Interfaces.ViewModels.Dialogs;

using Stylet;

namespace CineQuebec.Windows.ViewModels.Dialogs;

public class DialogNomAffichageViewModel : Screen, IDialogNomAffichageViewModel
{
    private string _nom = string.Empty;

    public bool AValide { get; private set; }

    public string Nom
    {
        get => _nom;
        set => SetAndNotify(ref _nom, value);
    }

    public void Annuler()
    {
        AValide = false;
        RequestClose(false);
    }

    public void Valider()
    {
        AValide = true;
        RequestClose(true);
    }
}