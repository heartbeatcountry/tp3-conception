using CineQuebec.Domain.Entities.Projections;

using Tests.Domain.Entities.Abstract;

namespace Tests.Domain.Entities.Projections;

public class BilletTests : EntiteTests<Billet>
{
    private static readonly Guid Projection1 = Guid.NewGuid();
    private static readonly Guid Utilisateur1 = Guid.NewGuid();

    protected override object?[] ArgsConstructeur =>
    [
        Projection1,
        Utilisateur1
    ];

    [Test]
    public void Constructor_WhenGivenValidProjection_ShouldSetBillet()
    {
        //Assert

        Assert.That(() => Entite.IdProjection, Is.EqualTo(Projection1));
    }


    [Test]
    public void Constructor_WhenGivenValidUtilisateur_ShouldSetBillet()
    {
        //Assert

        Assert.That(() => Entite.IdUtilisateur, Is.EqualTo(Utilisateur1));
    }
}