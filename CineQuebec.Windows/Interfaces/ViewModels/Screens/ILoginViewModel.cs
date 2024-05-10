using System.Windows;

using Stylet;

namespace CineQuebec.Windows.Interfaces.ViewModels.Screens;

public interface ILoginViewModel : IScreen
{
    Visibility VisibiliteTexteConnexion { get; }
    string NomUsager { get; set; }
    bool CanSeConnecter { get; }
    bool CanOuvrirInscription { get; }
    Task SeConnecter();
    void OuvrirInscription();
    void OnMdpChange(object sender, RoutedEventArgs _);
}