using System.Windows;
using System.Windows.Controls;
using CineQuebec.Application.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CineQuebec.Windows.View;

/// <summary>
///     Logique d'interaction pour AdminHomeControl.xaml
/// </summary>
public partial class AdminHomeControl : UserControl
{
    private readonly IFilmCreationService _filmCreationService;
    public AdminHomeControl(IFilmCreationService filmCreationService)
    {
        InitializeComponent();
        _filmCreationService = filmCreationService;
    }

    private void BoutonFilms_OnClick(object sender, RoutedEventArgs e)
    {
        var form = new FormAjoutFilm(_filmCreationService);
        ((MainWindow)System.Windows.Application.Current.MainWindow).mainContentControl.Content = form;
    }
}