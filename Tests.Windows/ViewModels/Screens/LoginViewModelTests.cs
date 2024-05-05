using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using CineQuebec.Windows.Interfaces.ViewModels.Dialogs;
using CineQuebec.Windows.ViewModels.Screens;

using Moq;

using Tests.Windows.ViewModels.Abstract;

namespace Tests.Windows.ViewModels.Screens;

public class LoginViewModelTests : GenericViewModelTests<LoginViewModel>
{
    [Test]
    public void OuvrirInscription_WhenCalled_ShouldOpenDialogInscriptionUtilisateur()
    {
        // Arrange
        Mock<IDialogInscriptionUtilisateurViewModel> mockDialogInscriptionUtilisateurViewModel = new();
        mockDialogInscriptionUtilisateurViewModel.SetupGet(diu => diu.InscriptionReussie)
            .Returns(false);
        DialogFactoryMock.Setup(d => d.CreateDialogInscriptionUtilisateur())
            .Returns(mockDialogInscriptionUtilisateurViewModel.Object);

        // Act
        ViewModel.OuvrirInscription();

        // Assert
        WindowManagerMock.Verify(w => w.ShowDialog(mockDialogInscriptionUtilisateurViewModel.Object));
    }

    [Test]
    public void OuvrirInscription_WhenInscriptionReussie_ShouldNavigateToHomeView()
    {
        // Arrange
        Mock<IDialogInscriptionUtilisateurViewModel> mockDialogInscriptionUtilisateurViewModel = new();
        mockDialogInscriptionUtilisateurViewModel.SetupGet(diu => diu.InscriptionReussie)
            .Returns(true);
        DialogFactoryMock.Setup(d => d.CreateDialogInscriptionUtilisateur())
            .Returns(mockDialogInscriptionUtilisateurViewModel.Object);

        // Act
        ViewModel.OuvrirInscription();

        // Assert
        NavigationControllerMock.Verify(n => n.NavigateTo<HomeViewModel>(null));
    }

    [Test]
    public void SeConnecter_WhenCalled_ShouldDisableGUI()
    {
        // Act
        ViewModel.SeConnecter().Wait();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(ViewModel.CanSeConnecter, Is.False);
            Assert.That(ViewModel.CanOuvrirInscription, Is.False);
            Assert.That(ViewModel.VisibiliteTexteConnexion, Is.EqualTo(Visibility.Visible));
            Assert.That(Mouse.OverrideCursor, Is.EqualTo(Cursors.Wait));
        });
    }

    [Test]
    public void SeConnecter_OnSubsequentCalls_ShouldNotShowDBConnectionMessage()
    {
        // Arrange
        UtilisateurAuthenticationServiceMock
            .Setup(a => a.AuthentifierThreadAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ThrowsAsync(new SecurityException());
        ViewModel.SeConnecter().Wait();
        UtilisateurAuthenticationServiceMock
            .Setup(a => a.AuthentifierThreadAsync(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        // Act
        ViewModel.SeConnecter().Wait();

        // Assert
        Assert.That(ViewModel.VisibiliteTexteConnexion, Is.EqualTo(Visibility.Hidden));
    }

    [Test]
    public void SeConnecter_WhenFailed_ShouldEnableGUI()
    {
        // Arrange
        UtilisateurAuthenticationServiceMock
            .Setup(a => a.AuthentifierThreadAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ThrowsAsync(new SecurityException());

        // Act
        ViewModel.SeConnecter().Wait();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(ViewModel.CanSeConnecter, Is.True);
            Assert.That(ViewModel.CanOuvrirInscription, Is.True);
            Assert.That(ViewModel.VisibiliteTexteConnexion, Is.EqualTo(Visibility.Hidden));
            Assert.That(Mouse.OverrideCursor, Is.Null);
        });
    }

    [Test]
    public void SeConnecter_WhenFailed_ShouldCallGestionnaireExceptions()
    {
        // Arrange
        SecurityException exception = new();
        UtilisateurAuthenticationServiceMock
            .Setup(a => a.AuthentifierThreadAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ThrowsAsync(exception);

        // Act
        ViewModel.SeConnecter().Wait();

        // Assert
        GestionnaireExceptionsMock.Verify(g => g.GererException(exception));
    }

    [Test]
    public void SeConnecter_WhenSuccessful_ShouldNavigateToHomeView()
    {
        // Act
        ViewModel.SeConnecter().Wait();

        // Assert
        NavigationControllerMock.Verify(n => n.NavigateTo<HomeViewModel>(null));
    }

    [Test]
    public void OnMdpChange_WhenCalled_ShouldSetMotDePasse()
    {
        // Arrange
        const string mdp = "mdp";
        PasswordBox passwordBox = new() { Password = mdp };

        // Act
        ViewModel.OnMdpChange(passwordBox, null!);
        ViewModel.SeConnecter().Wait();

        // Assert
        UtilisateurAuthenticationServiceMock.Verify(a => a.AuthentifierThreadAsync(It.IsAny<string>(), mdp));
    }

    [Test]
    public void NomUsager_WhenSet_ShouldSetNomUsager()
    {
        // Arrange
        const string nomUsager = "nomUsager";

        // Act
        ViewModel.NomUsager = nomUsager;
        ViewModel.SeConnecter().Wait();

        // Assert
        UtilisateurAuthenticationServiceMock.Verify(a => a.AuthentifierThreadAsync(nomUsager, It.IsAny<string>()));
    }
}