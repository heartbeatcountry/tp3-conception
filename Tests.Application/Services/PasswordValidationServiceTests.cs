using System.Security;

using CineQuebec.Application.Services;

namespace Tests.Application.Services;

internal class PasswordValidationServiceTests : GenericServiceTests<PasswordValidationService>
{
    private const string MotDePasseCommun = "administrator";
    private const string MotDePasseTropCourt = "12345";
    private const string MotDePasseValide = "j'aime le gratin dauphinois";
    private const string MotDePasseFaibleEntropie = "aaaaaaaaaa@$";
    private readonly string _motDePasseTropLong = new('x', PasswordValidationService.LongueurMaximaleMdp + 1);

    [Test]
    public void ValiderMdpEstSecuritaire_WhenGivenCommonPassword_ShouldThrowSecurityException()
    {
        // Act & Assert
        AggregateException? aggregateException = Assert.Throws<AggregateException>(() =>
            Service.ValiderMdpEstSecuritaire(MotDePasseCommun));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<SecurityException>().With.Message.Contains("trop commun"));
    }

    [Test]
    public void ValiderMdpEstSecuritaire_WhenGivenTooShortPassword_ShouldThrowSecurityException()
    {
        // Act & Assert
        AggregateException? aggregateException = Assert.Throws<AggregateException>(() =>
            Service.ValiderMdpEstSecuritaire(MotDePasseTropCourt));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<SecurityException>().With.Message.Contains("doit contenir au moins"));
    }

    [Test]
    public void ValiderMdpEstSecuritaire_WhenGivenTooLongPassword_ShouldThrowSecurityException()
    {
        // Act & Assert
        AggregateException? aggregateException = Assert.Throws<AggregateException>(() =>
            Service.ValiderMdpEstSecuritaire(_motDePasseTropLong));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<SecurityException>().With.Message.Contains("doit contenir au plus"));
    }

    [Test]
    public void ValiderMdpEstSecuritaire_WhenGivenWeakPassword_ShouldThrowSecurityException()
    {
        // Act & Assert
        AggregateException? aggregateException = Assert.Throws<AggregateException>(() =>
            Service.ValiderMdpEstSecuritaire(MotDePasseFaibleEntropie));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<SecurityException>().With.Message.Contains("caractères uniques"));
    }

    [Test]
    public void ValiderMdpEstSecuritaire_WhenGivenValidPassword_ShouldNotThrow()
    {
        // Act & Assert
        Assert.DoesNotThrow(() => Service.ValiderMdpEstSecuritaire(MotDePasseValide));
    }
}