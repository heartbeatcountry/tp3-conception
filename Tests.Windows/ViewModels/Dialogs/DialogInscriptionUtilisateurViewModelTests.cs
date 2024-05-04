using System.Security;

using CineQuebec.Windows.ViewModels.Dialogs;

using Moq;

using Tests.Windows.ViewModels.Abstract;

namespace Tests.Windows.ViewModels.Dialogs;

public class DialogInscriptionUtilisateurViewModelTests : GenericViewModelTests<DialogInscriptionUtilisateurViewModel>
{
    public const string Nom = "Nom";
    public const string Prenom = "Prenom";
    public const string Courriel = "courriel@domaine.com";
    private const string MotDePasse = "MotDePasse";
    private const string AutreMotDePasse = "AutreMotDePasse";

    [SetUp]
    public new void SetUp()
    {
        base.SetUp();

        ViewModel.Nom = Nom;
        ViewModel.Prenom = Prenom;
        ViewModel.Courriel = Courriel;
        ViewModel.MotDePasse = MotDePasse;
        ViewModel.ConfirmationMotDePasse = MotDePasse;
    }

    [Test]
    public void Valider_WhenPasswordsAreDifferent_ShouldCallGestionnaireExceptions()
    {
        // Arrange
        ViewModel.ConfirmationMotDePasse = AutreMotDePasse;

        // Act
        ViewModel.Valider().Wait();

        // Assert
        GestionnaireExceptionsMock.Verify(g =>
            g.GererException(It.IsAny<ArgumentException>()), Times.Once);
    }

    [Test]
    public void Valider_WhenCreationServiceThrows_ShouldCallGestionnaireExceptions()
    {
        // Arrange
        UtilisateurCreationServiceMock.Setup(u =>
                u.CreerUtilisateurAsync(Nom, Prenom, Courriel, MotDePasse))
            .ThrowsAsync(new SecurityException());

        // Act
        ViewModel.Valider().Wait();

        // Assert
        GestionnaireExceptionsMock.Verify(g =>
            g.GererException(It.IsAny<SecurityException>()), Times.Once);
    }

    [Test]
    public void Valider_WhenAuthenticationServiceThrows_ShouldCallGestionnaireExceptions()
    {
        // Arrange
        UtilisateurAuthenticationServiceMock.Setup(u =>
                u.AuthentifierThreadAsync(Courriel, MotDePasse))
            .ThrowsAsync(new SecurityException());

        // Act
        ViewModel.Valider().Wait();

        // Assert
        GestionnaireExceptionsMock.Verify(g =>
            g.GererException(It.IsAny<SecurityException>()), Times.Once);
    }

    [Test]
    public void Valider_WhenRegistrationIsSuccessful_ShouldSetInscriptionReussieToTrueAndCloseDialog()
    {
        // Act
        ViewModel.Valider().Wait();

        // Assert
        Assert.That(ViewModel.InscriptionReussie, Is.True);
        ConductorMock.Verify(c => c.CloseItem(ViewModel), Times.Once);
    }

    [Test]
    public void Annuler_ShouldCloseDialog()
    {
        // Act
        ViewModel.Annuler();

        // Assert
        Assert.That(ViewModel.InscriptionReussie, Is.False);
        ConductorMock.Verify(c => c.CloseItem(ViewModel), Times.Once);
    }
}