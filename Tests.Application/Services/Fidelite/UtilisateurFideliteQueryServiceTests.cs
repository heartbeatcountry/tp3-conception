using CineQuebec.Application.Services.Fidelite;
using CineQuebec.Domain.Interfaces.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Projections;
using CineQuebec.Domain.Interfaces.Entities.Utilisateur;

using Moq;

namespace Tests.Application.Services.Fidelite;

public class UtilisateurFideliteQueryServiceTests : GenericServiceTests<UtilisateurFideliteQueryService>
{
    private IBillet[] _lstBillets = null!;
    private IUtilisateur[] _lstUtilisateurs = null!;
    private IActeur _mockActeur = null!;
    private ICategorieFilm _mockCategorieFilm = null!;
    private IRealisateur _mockRealisateur = null!;

    [SetUp]
    public new void SetUp()
    {
        base.SetUp();

        _mockActeur = Mock.Of<IActeur>(a => a.Id == Guid.NewGuid());
        _mockRealisateur = Mock.Of<IRealisateur>(r => r.Id == Guid.NewGuid());
        _mockCategorieFilm = Mock.Of<ICategorieFilm>(c => c.Id == Guid.NewGuid());
        _lstUtilisateurs =
        [
            Mock.Of<IUtilisateur>(u => u.Id == Guid.NewGuid() &&
                                       u.ActeursFavorisParId == new[] { _mockActeur.Id }),
            Mock.Of<IUtilisateur>(u => u.Id == Guid.NewGuid() &&
                                       u.ActeursFavorisParId == new[] { _mockActeur.Id }),
            Mock.Of<IUtilisateur>(u => u.Id == Guid.NewGuid() &&
                                       u.RealisateursFavorisParId == new[] { _mockRealisateur.Id }),
            Mock.Of<IUtilisateur>(u => u.Id == Guid.NewGuid() &&
                                       u.RealisateursFavorisParId == new[] { _mockRealisateur.Id }),
            Mock.Of<IUtilisateur>(u => u.Id == Guid.NewGuid()
                                       && u.CategoriesPrefereesParId == new[] { _mockCategorieFilm.Id }),
            Mock.Of<IUtilisateur>(u => u.Id == Guid.NewGuid()
                                       && u.CategoriesPrefereesParId == new[] { _mockCategorieFilm.Id })
        ];
        _lstBillets =
        [
            Mock.Of<IBillet>(b => b.Id == Guid.NewGuid() && b.IdUtilisateur == _lstUtilisateurs[0].Id),
            Mock.Of<IBillet>(b => b.Id == Guid.NewGuid() && b.IdUtilisateur == _lstUtilisateurs[1].Id),
            Mock.Of<IBillet>(b => b.Id == Guid.NewGuid() && b.IdUtilisateur == _lstUtilisateurs[1].Id),
            Mock.Of<IBillet>(b => b.Id == Guid.NewGuid() && b.IdUtilisateur == _lstUtilisateurs[1].Id),

            Mock.Of<IBillet>(b => b.Id == Guid.NewGuid() && b.IdUtilisateur == _lstUtilisateurs[2].Id),
            Mock.Of<IBillet>(b => b.Id == Guid.NewGuid() && b.IdUtilisateur == _lstUtilisateurs[2].Id),
            Mock.Of<IBillet>(b => b.Id == Guid.NewGuid() && b.IdUtilisateur == _lstUtilisateurs[3].Id),
            Mock.Of<IBillet>(b => b.Id == Guid.NewGuid() && b.IdUtilisateur == _lstUtilisateurs[3].Id),
            Mock.Of<IBillet>(b => b.Id == Guid.NewGuid() && b.IdUtilisateur == _lstUtilisateurs[3].Id),
            Mock.Of<IBillet>(b => b.Id == Guid.NewGuid() && b.IdUtilisateur == _lstUtilisateurs[3].Id),

            Mock.Of<IBillet>(b => b.Id == Guid.NewGuid() && b.IdUtilisateur == _lstUtilisateurs[4].Id),
            Mock.Of<IBillet>(b => b.Id == Guid.NewGuid() && b.IdUtilisateur == _lstUtilisateurs[5].Id),
            Mock.Of<IBillet>(b => b.Id == Guid.NewGuid() && b.IdUtilisateur == _lstUtilisateurs[5].Id)
        ];
    }

    [Test]
    public void ObtenirUtilisateursFideles_WhenProjectionNotFound_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        ProjectionRepositoryMock.Setup(x => x.ObtenirParIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((IProjection?)null);

        // Act & Assert
        Assert.That(() => Service.ObtenirUtilisateursFideles(Guid.Empty), Throws.InstanceOf<KeyNotFoundException>());
    }

    [Test]
    public void ObtenirUtilisateursFideles_WhenFilmNotFound_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        ProjectionRepositoryMock.Setup(x => x.ObtenirParIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(Mock.Of<IProjection>(p => p.IdFilm == Guid.NewGuid()));

        FilmRepositoryMock.Setup(x => x.ObtenirParIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((IFilm?)null);

        // Act & Assert
        Assert.That(() => Service.ObtenirUtilisateursFideles(Guid.Empty), Throws.InstanceOf<KeyNotFoundException>());
    }

    [Test]
    public async Task ObtenirUtilisateursFideles_ProjectionEstAvantPremiere_ShouldReturnUtilisateursFideles()
    {
        // Arrange
        IProjection mockProjection = Mock.Of<IProjection>(p => p.EstAvantPremiere == true);
        IFilm mockFilm = Mock.Of<IFilm>(f =>
            f.Id == Guid.NewGuid() && f.RealisateursParId == new[] { _mockRealisateur.Id } &&
            f.ActeursParId == new[] { _mockActeur.Id });
        ProjectionRepositoryMock.Setup(x => x.ObtenirParIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(mockProjection);
        FilmRepositoryMock.Setup(x => x.ObtenirParIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(mockFilm);
        BilletRepositoryMock.Setup(br => br.ObtenirTousAsync(null, null))
            .ReturnsAsync(_lstBillets);
        UtilisateurRepositoryMock.Setup(ur => ur.ObtenirParIdAsync(_lstUtilisateurs[0].Id))
            .ReturnsAsync(_lstUtilisateurs[0]);
        UtilisateurRepositoryMock.Setup(ur => ur.ObtenirParIdAsync(_lstUtilisateurs[1].Id))
            .ReturnsAsync(_lstUtilisateurs[1]);
        UtilisateurRepositoryMock.Setup(ur => ur.ObtenirParIdAsync(_lstUtilisateurs[2].Id))
            .ReturnsAsync(_lstUtilisateurs[2]);
        UtilisateurRepositoryMock.Setup(ur => ur.ObtenirParIdAsync(_lstUtilisateurs[3].Id))
            .ReturnsAsync(_lstUtilisateurs[3]);

        Guid[] expectedUtilisateurs =
        [
            _lstUtilisateurs[3].Id,
            _lstUtilisateurs[1].Id,
            _lstUtilisateurs[2].Id,
            _lstUtilisateurs[0].Id
        ];

        // Act
        Guid[] actualUtilisateurs = (await Service.ObtenirUtilisateursFideles(Guid.Empty)).Select(u => u.Id).ToArray();

        // Assert
        Assert.That(actualUtilisateurs, Is.EqualTo(expectedUtilisateurs));
    }

    [Test]
    public async Task ObtenirUtilisateursFideles_ProjectionEstReprojection_ShouldReturnUtilisateursFideles()
    {
        // Arrange
        IProjection mockProjection = Mock.Of<IProjection>(p => p.EstAvantPremiere == false);
        IFilm mockFilm = Mock.Of<IFilm>(f =>
            f.Id == Guid.NewGuid() && f.IdCategorie == _mockCategorieFilm.Id);
        ProjectionRepositoryMock.Setup(x => x.ObtenirParIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(mockProjection);
        FilmRepositoryMock.Setup(x => x.ObtenirParIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(mockFilm);
        BilletRepositoryMock.Setup(br => br.ObtenirTousAsync(null, null))
            .ReturnsAsync(_lstBillets);
        UtilisateurRepositoryMock.Setup(ur => ur.ObtenirParIdAsync(_lstUtilisateurs[4].Id))
            .ReturnsAsync(_lstUtilisateurs[4]);
        UtilisateurRepositoryMock.Setup(ur => ur.ObtenirParIdAsync(_lstUtilisateurs[5].Id))
            .ReturnsAsync(_lstUtilisateurs[5]);

        Guid[] expectedUtilisateurs =
        [
            _lstUtilisateurs[5].Id,
            _lstUtilisateurs[4].Id
        ];


        // Act
        Guid[] actualUtilisateurs = (await Service.ObtenirUtilisateursFideles(Guid.Empty)).Select(u => u.Id).ToArray();

        // Assert
        Assert.That(actualUtilisateurs, Is.EqualTo(expectedUtilisateurs));
    }
}