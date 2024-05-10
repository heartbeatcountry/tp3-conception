using CineQuebec.Windows.Interfaces.ViewModels.Dialogs;

using Stylet;

namespace CineQuebec.Windows.ViewModels.Dialogs;

public class DialogNomPrenomViewModel : Screen, IDialogNomPrenomViewModel
{
    private string _nom = string.Empty;
    private string _prenom = string.Empty;

    public bool AValide { get; private set; }

    public string Nom
    {
        get => _nom;
        set => SetAndNotify(ref _nom, value);
    }

    public string Prenom
    {
        get => _prenom;
        set => SetAndNotify(ref _prenom, value);
    }

    public void Annuler()
    {
        RequestClose(false);
    }

    public void Valider()
    {
        AValide = true;
        RequestClose(true);
    }
}