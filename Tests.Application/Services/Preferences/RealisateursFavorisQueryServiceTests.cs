using CineQuebec.Application.Records.Films;
using CineQuebec.Application.Services.Preferences;
using CineQuebec.Domain.Interfaces.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Utilisateur;

using Moq;

namespace Tests.Application.Services.Preferences;

public class RealisateursFavorisQueryServiceTests : GenericServiceTests<RealisateursFavorisQueryService>
{
    private readonly Guid _idUtilisateur = Guid.NewGuid();
    private readonly IRealisateur _realisateur1 = Mock.Of<IRealisateur>(a => a.Id == Guid.NewGuid());
    private readonly IRealisateur _realisateur2 = Mock.Of<IRealisateur>(a => a.Id == Guid.NewGuid());
    private readonly IRealisateur _realisateur3 = Mock.Of<IRealisateur>(a => a.Id == Guid.NewGuid());
    private readonly Mock<IUtilisateur> _utilisateurMock = new();

    [SetUp]
    public new void SetUp()
    {
        base.SetUp();

        _utilisateurMock.SetupGet(u => u.Id).Returns(_idUtilisateur);
        _utilisateurMock.SetupGet(u => u.RealisateursFavorisParId).Returns([
            _realisateur1.Id, _realisateur2.Id, _realisateur3.Id
        ]);

        RealisateurRepositoryMock.Setup(ar =>
                ar.ObtenirParIdsAsync(new[] { _realisateur1.Id, _realisateur2.Id, _realisateur3.Id }))
            .ReturnsAsync([_realisateur1, _realisateur2, _realisateur3]);

        UtilisateurAuthenticationServiceMock.Setup(uas => uas.ObtenirIdUtilisateurConnecte())
            .Returns(_idUtilisateur);

        UtilisateurRepositoryMock.Setup(ur => ur.ObtenirParIdAsync(_idUtilisateur))
            .ReturnsAsync(_utilisateurMock.Object);
    }

    [Test]
    public async Task ObtenirRealisateursFavoris_WhenConnected_ReturnsListOfRealisateurs()
    {
        // Act
        IEnumerable<RealisateurDto> realisateurs = await Service.ObtenirRealisateursFavoris();

        // Assert
        Assert.That(realisateurs,
            Is.EquivalentTo(new RealisateurDto[]
            {
                new(_realisateur1.Id, _realisateur1.Prenom, _realisateur1.Nom),
                new(_realisateur2.Id, _realisateur2.Prenom, _realisateur2.Nom),
                new(_realisateur3.Id, _realisateur3.Prenom, _realisateur3.Nom)
            }));
    }
}