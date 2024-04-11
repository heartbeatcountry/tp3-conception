using Stylet;

namespace CineQuebec.Windows.Views;

public class DialogNomAffichageViewModel : Screen
{
    private string _nom = String.Empty;

    public bool AValide { get; private set; }

    public string Nom
    {
        get => _nom;
        set => SetAndNotify(ref _nom, value);
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