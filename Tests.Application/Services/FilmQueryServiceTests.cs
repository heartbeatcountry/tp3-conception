using CineQuebec.Application.Records.Films;
using CineQuebec.Application.Services;
using CineQuebec.Domain.Interfaces.Entities.Films;

using Moq;

namespace Tests.Application.Services;

public class FilmQueryServiceTests : GenericServiceTests<FilmQueryService>
{
    [Test]
    public void ObtenirDetailsFilmParId_WhenFilmDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        Guid id = Guid.NewGuid();
        FilmRepositoryMock.Setup(r => r.ObtenirParIdAsync(id)).ReturnsAsync((IFilm?)null);

        // Act
        FilmDto? filmDto = Service.ObtenirDetailsFilmParId(id).Result;

        // Assert
        Assert.That(filmDto, Is.Null);
    }

    [Test]
    public void ObtenirDetailsFilmParId_WhenFilmExists_ShouldReturnFilmDetails()
    {
        // Arrange
        Guid id = Guid.NewGuid();
        IFilm film = Mock.Of<IFilm>();
        FilmRepositoryMock.Setup(r => r.ObtenirParIdAsync(id)).ReturnsAsync(film);
        ICategorieFilm categorie = Mock.Of<ICategorieFilm>();
        CategorieFilmRepositoryMock.Setup(r => r.ObtenirParIdAsync(film.IdCategorie)).ReturnsAsync(categorie);
        IEnumerable<IRealisateur> realisateurs = new[] { Mock.Of<IRealisateur>(), Mock.Of<IRealisateur>() };
        RealisateurRepositoryMock.Setup(r => r.ObtenirParIdsAsync(film.RealisateursParId)).ReturnsAsync(realisateurs);
        IEnumerable<IActeur> acteurs = new[] { Mock.Of<IActeur>(), Mock.Of<IActeur>() };
        ActeurRepositoryMock.Setup(r => r.ObtenirParIdsAsync(film.ActeursParId)).ReturnsAsync(acteurs);

        // Act
        FilmDto? filmDto = Service.ObtenirDetailsFilmParId(id).Result;

        // Assert
        Assert.That(filmDto, Is.Not.Null);
    }

    [Test]
    public void ObtenirTous_Always_ShouldReturnAllFilms()
    {
        // Arrange
        IEnumerable<IFilm> films = new[] { Mock.Of<IFilm>(), Mock.Of<IFilm>(), Mock.Of<IFilm>() };
        FilmRepositoryMock.Setup(r => r.ObtenirTousAsync(null, null)).ReturnsAsync(films);

        // Act
        IEnumerable<FilmDto> filmsDto = Service.ObtenirTous().Result;

        // Assert
        Assert.That(filmsDto.Count(), Is.EqualTo(3));
    }
}