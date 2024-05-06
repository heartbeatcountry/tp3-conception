using System.Linq.Expressions;

using CineQuebec.Application.Records.Projections;
using CineQuebec.Application.Services.Projections;
using CineQuebec.Domain.Interfaces.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Projections;

using Moq;

namespace Tests.Application.Services.Projections;

public class ProjectionQueryServiceTests : GenericServiceTests<ProjectionQueryService>
{
    [Test]
    public async Task ObtenirProjectionsAVenirPourFilm_WhenFilmDoesNotExist_ShouldReturnEmptyList()
    {
        // Arrange
        Guid idFilm = Guid.NewGuid();
        FilmRepositoryMock.Setup(r => r.ObtenirParIdAsync(idFilm)).ReturnsAsync((IFilm?)null);

        // Act
        IEnumerable<ProjectionDto> result = await Service.ObtenirProjectionsAVenirPourFilm(idFilm);

        // Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task ObtenirProjectionsAVenirPourFilm_WhenFilmHasNoProjection_ShouldReturnEmptyList()
    {
        // Arrange
        Guid idFilm = Guid.NewGuid();
        IFilm film = Mock.Of<IFilm>(f => f.Id == idFilm);
        FilmRepositoryMock.Setup(r => r.ObtenirParIdAsync(idFilm)).ReturnsAsync(film);
        ProjectionRepositoryMock.Setup(r => r.ObtenirTousAsync(It.IsAny<Expression<Func<IProjection, bool>>>(),
                It.IsAny<Func<IQueryable<IProjection>, IOrderedQueryable<IProjection>>>()))
            .ReturnsAsync(new List<IProjection>());

        // Act
        IEnumerable<ProjectionDto> result = await Service.ObtenirProjectionsAVenirPourFilm(idFilm);

        // Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task ObtenirProjectionsAVenirPourFilm_WhenFilmHasProjections_ShouldReturnProjectionsOrderedByDate()
    {
        // Arrange
        Guid idFilm = Guid.NewGuid();
        Guid idSalle = Guid.NewGuid();
        IFilm film = Mock.Of<IFilm>(f => f.Id == idFilm);
        ISalle salle = Mock.Of<ISalle>(s => s.Id == idSalle);
        List<ISalle> lstSalles = [salle];
        List<IProjection> projections =
        [
            Mock.Of<IProjection>(pf => pf.Id == Guid.NewGuid() && pf.IdFilm == idFilm && pf.IdSalle == idSalle &&
                                       pf.DateHeure == DateTime.Now.AddDays(1)),

            Mock.Of<IProjection>(pf => pf.Id == Guid.NewGuid() && pf.IdFilm == idFilm && pf.IdSalle == idSalle &&
                                       pf.DateHeure == DateTime.Now.AddDays(2)),

            Mock.Of<IProjection>(pf => pf.Id == Guid.NewGuid() && pf.IdFilm == idFilm && pf.IdSalle == idSalle &&
                                       pf.DateHeure == DateTime.Now.AddDays(3))
        ];
        FilmRepositoryMock.Setup(r => r.ObtenirParIdAsync(idFilm)).ReturnsAsync(film);
        SalleRepositoryMock.Setup(r => r.ObtenirParIdsAsync(It.IsAny<IEnumerable<Guid>>())).ReturnsAsync(lstSalles);
        ProjectionRepositoryMock.Setup(r => r.ObtenirTousAsync(It.IsAny<Expression<Func<IProjection, bool>>>(),
                It.IsAny<Func<IQueryable<IProjection>, IOrderedQueryable<IProjection>>>()))
            .ReturnsAsync(projections);

        // Act
        IEnumerable<ProjectionDto> result = await Service.ObtenirProjectionsAVenirPourFilm(idFilm);
        IEnumerable<ProjectionDto> projectionDtos = result as ProjectionDto[] ?? result.ToArray();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(projectionDtos, Has.Exactly(3).Items);
            Assert.That(projectionDtos, Is.Ordered.By(nameof(ProjectionDto.DateHeure)));
            Assert.That(projectionDtos.ElementAt(0).DateHeure, Is.EqualTo(projections.ElementAt(0).DateHeure));
            Assert.That(projectionDtos.ElementAt(1).DateHeure, Is.EqualTo(projections.ElementAt(1).DateHeure));
            Assert.That(projectionDtos.ElementAt(2).DateHeure, Is.EqualTo(projections.ElementAt(2).DateHeure));
        });
    }
}