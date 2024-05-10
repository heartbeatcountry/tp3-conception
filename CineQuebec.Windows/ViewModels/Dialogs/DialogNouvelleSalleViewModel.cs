using CineQuebec.Windows.Interfaces.ViewModels.Dialogs;

using Stylet;

namespace CineQuebec.Windows.ViewModels.Dialogs;

public class DialogNouvelleSalleViewModel : Screen, IDialogNouvelleSalleViewModel
{
    private string _capaciteStr = string.Empty;
    private string _numeroStr = string.Empty;

    public bool AValide { get; private set; }

    public string NumeroStr
    {
        get => _numeroStr;
        set => SetAndNotify(ref _numeroStr, value);
    }

    public string CapaciteStr
    {
        get => _capaciteStr;
        set => SetAndNotify(ref _capaciteStr, value);
    }

    public byte Numero => byte.TryParse(NumeroStr, out byte numero) ? numero : (byte)0;
    public ushort Capacite => ushort.TryParse(CapaciteStr, out ushort capacite) ? capacite : (ushort)0;

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