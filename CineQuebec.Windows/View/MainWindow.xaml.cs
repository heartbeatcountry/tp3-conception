using System.Windows;
using System.Windows.Controls;
using CineQuebec.Application.Interfaces.Services;

namespace CineQuebec.Windows.View;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly IFilmCreationService _filmCreationService;

    public MainWindow(IFilmCreationService filmCreationService)
    {
        InitializeComponent();
        _filmCreationService = filmCreationService;
        mainContentControl.Content = new ConnexionControl();
    }

    public void AdminHomeControl()
    {
        mainContentControl.Content = new AdminHomeControl(_filmCreationService);
    }
}