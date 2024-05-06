using CineQuebec.Application.Records.Films;
using CineQuebec.Application.Services.Preferences;
using CineQuebec.Domain.Interfaces.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Utilisateur;

using Moq;

namespace Tests.Application.Services.Preferences;

public class CategoriesPrefereesQueryServiceTests : GenericServiceTests<CategoriesPrefereesQueryService>
{
    private readonly ICategorieFilm _categorie1 = Mock.Of<ICategorieFilm>(a => a.Id == Guid.NewGuid());
    private readonly ICategorieFilm _categorie2 = Mock.Of<ICategorieFilm>(a => a.Id == Guid.NewGuid());
    private readonly ICategorieFilm _categorie3 = Mock.Of<ICategorieFilm>(a => a.Id == Guid.NewGuid());
    private readonly Guid _idUtilisateur = Guid.NewGuid();
    private readonly Mock<IUtilisateur> _utilisateurMock = new();

    [SetUp]
    public new void SetUp()
    {
        base.SetUp();

        _utilisateurMock.SetupGet(u => u.Id).Returns(_idUtilisateur);
        _utilisateurMock.SetupGet(u => u.CategoriesPrefereesParId).Returns([
            _categorie1.Id, _categorie2.Id, _categorie3.Id
        ]);

        CategorieFilmRepositoryMock.Setup(ar =>
                ar.ObtenirParIdsAsync(new[] { _categorie1.Id, _categorie2.Id, _categorie3.Id }))
            .ReturnsAsync([_categorie1, _categorie2, _categorie3]);

        UtilisateurAuthenticationServiceMock.Setup(uas => uas.ObtenirIdUtilisateurConnecte())
            .Returns(_idUtilisateur);

        UtilisateurRepositoryMock.Setup(ur => ur.ObtenirParIdAsync(_idUtilisateur))
            .ReturnsAsync(_utilisateurMock.Object);
    }

    [Test]
    public async Task ObtenirCategoriesPreferees_WhenConnected_ReturnsListOfCategories()
    {
        // Act
        IEnumerable<CategorieFilmDto> categories = await Service.ObtenirCategoriesPreferees();

        // Assert
        Assert.That(categories,
            Is.EquivalentTo(new CategorieFilmDto[]
            {
                new(_categorie1.Id, _categorie1.NomAffichage), new(_categorie2.Id, _categorie2.NomAffichage),
                new(_categorie3.Id, _categorie3.NomAffichage)
            }));
    }
}