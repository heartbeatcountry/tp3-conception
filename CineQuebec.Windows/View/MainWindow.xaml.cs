namespace CineQuebec.Windows.View;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        mainContentControl.Content = new ConnexionControl();
    }

    public void AdminHomeControl()
    {
        mainContentControl.Content = new AdminHomeControl();
    }
}