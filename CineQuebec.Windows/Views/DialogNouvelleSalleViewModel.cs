using Stylet;

namespace CineQuebec.Windows.Views;

public class DialogNouvelleSalleViewModel : Screen
{
    private string _numeroStr = String.Empty;
    private string _capaciteStr = String.Empty;

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

    public byte Numero => Byte.TryParse(NumeroStr, out byte numero) ? numero : (byte)0;
    public ushort Capacite => UInt16.TryParse(CapaciteStr, out ushort capacite) ? capacite : (ushort)0;

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