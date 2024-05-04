using CineQuebec.Windows.ViewModels.Dialogs;

using Moq;

using Tests.Windows.ViewModels.Abstract;

namespace Tests.Windows.ViewModels.Dialogs;

public class DialogNomPrenomViewModelTests : GenericViewModelTests<DialogNomPrenomViewModel>
{
    [Test]
    public void Annuler_WhenCalled_ShouldCloseDialog()
    {
        // Act
        ViewModel.Annuler();

        // Assert
        Assert.That(ViewModel.AValide, Is.False);
        ConductorMock.Verify(c => c.CloseItem(ViewModel), Times.Once);
    }

    [Test]
    public void Valider_WhenCalled_ShouldCloseDialog()
    {
        // Act
        ViewModel.Valider();

        // Assert
        Assert.That(ViewModel.AValide, Is.True);
        ConductorMock.Verify(c => c.CloseItem(ViewModel), Times.Once);
    }
}