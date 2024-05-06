using CineQuebec.Application.Records.Films;
using CineQuebec.Application.Services.Preferences;
using CineQuebec.Domain.Interfaces.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Utilisateur;

using Moq;

namespace Tests.Application.Services.Preferences;

public class ActeursFavorisQueryServiceTests : GenericServiceTests<ActeursFavorisQueryService>
{
    private readonly IActeur _acteur1 = Mock.Of<IActeur>(a => a.Id == Guid.NewGuid());
    private readonly IActeur _acteur2 = Mock.Of<IActeur>(a => a.Id == Guid.NewGuid());
    private readonly IActeur _acteur3 = Mock.Of<IActeur>(a => a.Id == Guid.NewGuid());
    private readonly Guid _idUtilisateur = Guid.NewGuid();
    private readonly Mock<IUtilisateur> _utilisateurMock = new();

    [SetUp]
    public new void SetUp()
    {
        base.SetUp();

        _utilisateurMock.SetupGet(u => u.Id).Returns(_idUtilisateur);
        _utilisateurMock.SetupGet(u => u.ActeursFavorisParId).Returns([
            _acteur1.Id, _acteur2.Id, _acteur3.Id
        ]);

        ActeurRepositoryMock.Setup(ar => ar.ObtenirParIdsAsync(new[] { _acteur1.Id, _acteur2.Id, _acteur3.Id }))
            .ReturnsAsync([_acteur1, _acteur2, _acteur3]);

        UtilisateurAuthenticationServiceMock.Setup(uas => uas.ObtenirIdUtilisateurConnecte())
            .Returns(_idUtilisateur);

        UtilisateurRepositoryMock.Setup(ur => ur.ObtenirParIdAsync(_idUtilisateur))
            .ReturnsAsync(_utilisateurMock.Object);
    }

    [Test]
    public async Task ObtenirActeursFavoris_WhenConnected_ReturnsListOfActeurs()
    {
        // Act
        IEnumerable<ActeurDto> acteurs = await Service.ObtenirActeursFavoris();

        // Assert
        Assert.That(acteurs,
            Is.EquivalentTo(new ActeurDto[]
            {
                new(_acteur1.Id, _acteur1.Prenom, _acteur1.Nom), new(_acteur2.Id, _acteur2.Prenom, _acteur2.Nom),
                new(_acteur3.Id, _acteur3.Prenom, _acteur3.Nom)
            }));
    }
}