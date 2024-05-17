using System.Linq.Expressions;

using CineQuebec.Application.Records.Films;
using CineQuebec.Application.Services.Films;
using CineQuebec.Domain.Interfaces.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Projections;

using Moq;

namespace Tests.Application.Services.Films;

public class FilmQueryServiceTests : GenericServiceTests<FilmQueryService>
{
    [Test]
    public async Task ObtenirDetailsFilmParId_WhenFilmDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        Guid id = Guid.NewGuid();
        FilmRepositoryMock.Setup(r => r.ObtenirParIdAsync(id)).ReturnsAsync((IFilm?)null);

        // Act
        FilmDto? filmDto = await Service.ObtenirDetailsFilmParId(id);

        // Assert
        Assert.That(filmDto, Is.Null);
    }

    [Test]
    public async Task ObtenirDetailsFilmParId_WhenFilmExists_ShouldReturnFilmDetails()
    {
        // Arrange
        Guid id = Guid.NewGuid();
        IFilm film = Mock.Of<IFilm>();
        FilmRepositoryMock.Setup(r => r.ObtenirParIdAsync(id)).ReturnsAsync(film);
        ICategorieFilm categorie = Mock.Of<ICategorieFilm>();
        CategorieFilmRepositoryMock.Setup(r => r.ObtenirParIdAsync(film.IdCategorie)).ReturnsAsync(categorie);
        IEnumerable<IRealisateur> realisateurs =
        [
            Mock.Of<IRealisateur>(a => a.Prenom == "A" && a.Nom == "A"),
            Mock.Of<IRealisateur>(a => a.Prenom == "A" && a.Nom == "B"),
            Mock.Of<IRealisateur>(a => a.Prenom == "B" && a.Nom == "A"),
            Mock.Of<IRealisateur>(a => a.Prenom == "B" && a.Nom == "B")
        ];
        RealisateurRepositoryMock.Setup(r => r.ObtenirParIdsAsync(film.RealisateursParId)).ReturnsAsync(realisateurs);
        IEnumerable<IActeur> acteurs =
        [
            Mock.Of<IActeur>(a => a.Prenom == "A" && a.Nom == "A"),
            Mock.Of<IActeur>(a => a.Prenom == "A" && a.Nom == "B"),
            Mock.Of<IActeur>(a => a.Prenom == "B" && a.Nom == "A"),
            Mock.Of<IActeur>(a => a.Prenom == "B" && a.Nom == "B")
        ];
        ActeurRepositoryMock.Setup(r => r.ObtenirParIdsAsync(film.ActeursParId)).ReturnsAsync(acteurs);

        // Act
        FilmDto? filmDto = await Service.ObtenirDetailsFilmParId(id);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(filmDto, Is.Not.Null);
            Assert.That(filmDto!.Realisateurs, Has.Exactly(4).Items);
            Assert.That(filmDto.Realisateurs,
                Is.Ordered.By(nameof(RealisateurDto.Prenom)).Then.By(nameof(RealisateurDto.Nom)));
            Assert.That(filmDto.Realisateurs.ElementAt(0).Prenom, Is.EqualTo("A"));
            Assert.That(filmDto.Realisateurs.ElementAt(0).Nom, Is.EqualTo("A"));
            Assert.That(filmDto.Realisateurs.ElementAt(1).Prenom, Is.EqualTo("A"));
            Assert.That(filmDto.Realisateurs.ElementAt(1).Nom, Is.EqualTo("B"));
            Assert.That(filmDto.Realisateurs.ElementAt(2).Prenom, Is.EqualTo("B"));
            Assert.That(filmDto.Realisateurs.ElementAt(2).Nom, Is.EqualTo("A"));
            Assert.That(filmDto.Realisateurs.ElementAt(3).Prenom, Is.EqualTo("B"));
            Assert.That(filmDto.Realisateurs.ElementAt(3).Nom, Is.EqualTo("B"));
            Assert.That(filmDto.Acteurs, Has.Exactly(4).Items);
            Assert.That(filmDto.Acteurs, Is.Ordered.By(nameof(ActeurDto.Prenom)).Then.By(nameof(ActeurDto.Nom)));
            Assert.That(filmDto.Acteurs.ElementAt(0).Prenom, Is.EqualTo("A"));
            Assert.That(filmDto.Acteurs.ElementAt(0).Nom, Is.EqualTo("A"));
            Assert.That(filmDto.Acteurs.ElementAt(1).Prenom, Is.EqualTo("A"));
            Assert.That(filmDto.Acteurs.ElementAt(1).Nom, Is.EqualTo("B"));
            Assert.That(filmDto.Acteurs.ElementAt(2).Prenom, Is.EqualTo("B"));
            Assert.That(filmDto.Acteurs.ElementAt(2).Nom, Is.EqualTo("A"));
            Assert.That(filmDto.Acteurs.ElementAt(3).Prenom, Is.EqualTo("B"));
            Assert.That(filmDto.Acteurs.ElementAt(3).Nom, Is.EqualTo("B"));
        });
    }

    [Test]
    public async Task ObtenirTous_WhenFilmsExist_ShouldReturnAllFilmsOrderedByTitre()
    {
        // Arrange
        IEnumerable<IFilm> films =
        [
            Mock.Of<IFilm>(f => f.Titre == "A"), Mock.Of<IFilm>(f => f.Titre == "B"),
            Mock.Of<IFilm>(f => f.Titre == "C")
        ];
        FilmRepositoryMock.Setup(r => r.ObtenirTousAsync(null, null)).ReturnsAsync(films);

        // Act
        IEnumerable<FilmDto> filmsDto = (await Service.ObtenirTous()).ToArray();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(filmsDto, Has.Exactly(3).Items);
            Assert.That(filmsDto, Is.Ordered.By(nameof(FilmDto.Titre)));
            Assert.That(filmsDto.ElementAt(0).Titre, Is.EqualTo("A"));
            Assert.That(filmsDto.ElementAt(1).Titre, Is.EqualTo("B"));
            Assert.That(filmsDto.ElementAt(2).Titre, Is.EqualTo("C"));
        });
    }

    [Test]
    public async Task ObtenirTous_WhenNoFilmExists_ShouldReturnEmptyList()
    {
        // Arrange
        IEnumerable<IFilm> films = [];
        FilmRepositoryMock.Setup(r => r.ObtenirTousAsync(null, null)).ReturnsAsync(films);

        // Act
        IEnumerable<FilmDto> filmsDto = await Service.ObtenirTous();

        // Assert
        Assert.That(filmsDto, Is.Empty);
    }

    [Test]
    public async Task
        ObtenirTousAlAffiche_WhenFilmsAreCurrentlyPlaying_ShouldReturnAllFilmsCurrentlyPlayingOrderedByDateHeure()
    {
        // Arrange
        IFilm[] films =
        [
            Mock.Of<IFilm>(f => f.Id == Guid.NewGuid()), Mock.Of<IFilm>(f => f.Id == Guid.NewGuid()),
            Mock.Of<IFilm>(f => f.Id == Guid.NewGuid())
        ];
        IProjection[] projections =
        [
            Mock.Of<IProjection>(p => p.DateHeure == DateTime.Now.AddDays(-1) && p.IdFilm == films.ElementAt(0).Id),
            Mock.Of<IProjection>(p => p.DateHeure == DateTime.Now && p.IdFilm == films.ElementAt(1).Id),
            Mock.Of<IProjection>(p => p.DateHeure == DateTime.Now.AddDays(1) && p.IdFilm == films.ElementAt(2).Id)
        ];
        ProjectionRepositoryMock.Setup(r => r.ObtenirTousAsync(It.IsAny<Expression<Func<IProjection, bool>>>(),
            It.IsAny<Func<IQueryable<IProjection>, IOrderedQueryable<IProjection>>>())).ReturnsAsync(projections);
        FilmRepositoryMock.Setup(r => r.ObtenirParIdsAsync(It.IsAny<IEnumerable<Guid>>())).ReturnsAsync(films);

        // Act
        FilmDto[] filmsDto = (await Service.ObtenirTousAlAffiche()).ToArray();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(filmsDto, Has.Exactly(3).Items);
            Assert.That(filmsDto.ElementAt(0).Id, Is.EqualTo(films.ElementAt(0).Id));
            Assert.That(filmsDto.ElementAt(1).Id, Is.EqualTo(films.ElementAt(1).Id));
            Assert.That(filmsDto.ElementAt(2).Id, Is.EqualTo(films.ElementAt(2).Id));
        });
    }

    [Test]
    public async Task ObtenirTousAlAffiche_WhenNoFilmIsCurrentlyPlaying_ShouldReturnEmptyList()
    {
        // Arrange
        ProjectionRepositoryMock.Setup(r => r.ObtenirTousAsync(It.IsAny<Expression<Func<IProjection, bool>>>(),
            It.IsAny<Func<IQueryable<IProjection>, IOrderedQueryable<IProjection>>>())).ReturnsAsync([]);

        // Act
        IEnumerable<FilmDto> filmsDto = await Service.ObtenirTousAlAffiche();

        // Assert
        Assert.That(filmsDto, Is.Empty);
    }
}