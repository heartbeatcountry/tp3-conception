using Stylet;

namespace CineQuebec.Windows.Views;

public class DialogNomPrenomViewModel : Screen
{
    private string _nom = String.Empty;
    private string _prenom = String.Empty;

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