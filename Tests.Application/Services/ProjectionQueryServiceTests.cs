using System.Linq.Expressions;

using CineQuebec.Application.Records.Projections;
using CineQuebec.Application.Services;
using CineQuebec.Domain.Interfaces.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Projections;

using Moq;

namespace Tests.Application.Services;

public class ProjectionQueryServiceTests : GenericServiceTests<ProjectionQueryService>
{
    [Test]
    public void ObtenirProjectionsAVenirPourFilm_Always_ShouldReturnProjections()
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
        IEnumerable<ProjectionDto> result = Service.ObtenirProjectionsAVenirPourFilm(idFilm).Result;

        // Assert
        Assert.That(result.Count(), Is.EqualTo(3));
    }

    [Test]
    public void ObtenirProjectionsAVenirPourFilm_WhenFilmDoesNotExist_ShouldReturnEmptyList()
    {
        // Arrange
        Guid idFilm = Guid.NewGuid();
        FilmRepositoryMock.Setup(r => r.ObtenirParIdAsync(idFilm)).ReturnsAsync((IFilm?)null);

        // Act
        IEnumerable<ProjectionDto> result = Service.ObtenirProjectionsAVenirPourFilm(idFilm).Result;

        // Assert
        Assert.That(result.Count(), Is.EqualTo(0));
    }
}