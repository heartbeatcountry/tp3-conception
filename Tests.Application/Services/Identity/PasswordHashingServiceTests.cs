using CineQuebec.Application.Services.Identity;

namespace Tests.Application.Services.Identity;

public class PasswordHashingServiceTests : GenericServiceTests<PasswordHashingService>
{
    private const string MdpClair = "mdp";

    private const string MdpHacheValide =
        "$argon2id$v=19$m=64,t=3,p=1$GJhjQN96KgYxbj5Xa5i1fw$bqqBC+QKim5N9FHtOV1PjBaQIKg1LaKz6wrXPqdFfaE";

    private const string MdpHacheValideBesoinRehash =
        "$argon2id$v=19$m=64,t=1,p=1$M8WLGJ40QKCpXfrgcDpzig$+5O4dTmHSCxx9aRAKrHe8I4y6dfdF0ikHVExp4BKgbw";

    private const string MdpHacheInvalide =
        "$argon2id$v=19$m=64,t=3,p=1$rmSQVYF+uuYBQkZoA94Chg$N3dicFzMhKKIorjLvWc6PHc4zhbBDoCvcLM56yeoKCE";

    [Test]
    public void HacherMdp_WhenGivenPlainMdp_ShouldReturnHashedMdp()
    {
        // Act
        string result = Service.HacherMdp(MdpClair);

        // Assert
        Assert.That(result, Does.StartWith("$argon2id$v=19$m=64,t=3,p=1$"));
    }

    [Test]
    public void ValiderMdp_WhenGivenValidMdp_ShouldReturnTrue()
    {
        // Act
        bool result = Service.ValiderMdp(MdpClair, MdpHacheValide);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void ValiderMdp_WhenGivenInvalidMdp_ShouldReturnFalse()
    {
        // Act
        bool result = Service.ValiderMdp(MdpClair, MdpHacheInvalide);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void DoitEtreRehache_WhenGivenHashNeedsRehash_ShouldReturnTrue()
    {
        // Act
        bool result = Service.DoitEtreRehache(MdpHacheValideBesoinRehash);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void DoitEtreRehache_WhenGivenHashDoesNotNeedRehash_ShouldReturnFalse()
    {
        // Act
        bool result = Service.DoitEtreRehache(MdpHacheValide);

        // Assert
        Assert.That(result, Is.False);
    }
}