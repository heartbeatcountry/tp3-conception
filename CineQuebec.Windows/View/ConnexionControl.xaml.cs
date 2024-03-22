using System.Windows;
using System.Windows.Controls;

namespace CineQuebec.Windows.View;

/// <summary>
///     Logique d'interaction pour ConnexionControl.xaml
/// </summary>
public partial class ConnexionControl : UserControl
{
    public ConnexionControl()
    {
        InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        ((MainWindow)Application.Current.MainWindow).AdminHomeControl();
    }
}