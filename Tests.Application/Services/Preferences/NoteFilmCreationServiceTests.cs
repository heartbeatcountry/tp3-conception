using System.Linq.Expressions;

using CineQuebec.Application.Services.Preferences;
using CineQuebec.Domain.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Utilisateur;

using Moq;

namespace Tests.Application.Services;

public class NoteFilmCreationServiceTests : GenericServiceTests<NoteFilmCreationService>
{
    private static readonly Guid IdFilmValide = Guid.NewGuid();
    private static readonly Guid IdUtilisateurValide = Guid.NewGuid();
    private static readonly byte NoteValide = 7;
    private static readonly byte NoteInvalide = 14;


    [Test]
    public void CreerNoteFilm_WhenGivenInvalidFilm_ThrowsAggregateExceptionContainingArgumentException()
    {
        //Arrange
        FilmRepositoryMock.Setup(r => r.ObtenirParIdAsync(IdFilmValide))
            .ReturnsAsync((Film?)null);

        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            Service.NoterFilm(IdFilmValide, NoteValide));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("n'existe pas"));
    }


    [Test]
    public void CreerNoteFilm_WhenGivenIdFilmAndIdUtilisateurAlreadyPrensentInRepository_ShouldEditNoteFilm()
    {
        //Arrange
        NoteFilmRepositoryMock.Setup(r => r.ExisteAsync(It.IsAny<Expression<Func<INoteFilm, bool>>>()))
            .ReturnsAsync(true);
        FilmRepositoryMock.Setup(r => r.ObtenirParIdAsync(IdFilmValide))
            .ReturnsAsync(Mock.Of<IFilm>(cf => cf.Id == IdFilmValide));
        UtilisateurRepositoryMock.Setup(r => r.ObtenirParIdAsync(IdUtilisateurValide))
            .ReturnsAsync(Mock.Of<IUtilisateur>(cf => cf.Id == IdUtilisateurValide));

        // Act
        Assert.DoesNotThrowAsync(() => Service.NoterFilm(IdFilmValide, NoteValide));

        // Assert
        NoteFilmRepositoryMock.Verify(r => r.Modifier(It.IsAny<INoteFilm>()), Times.Once);
        UnitOfWorkMock.Verify(uow => uow.SauvegarderAsync(It.IsAny<CancellationToken?>()), Times.Once);
    }


    [Test]
    public void CreerNoteFilm_WhenGivenInvalidNote_ThrowsAggregateExceptionContainingArgumentException()
    {
        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            Service.NoterFilm(IdFilmValide, NoteInvalide));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message
                .Contains($"La note doit être comprise entre {NoteFilm.NoteMinimum} et {NoteFilm.NoteMaximum}."));
    }

    [SetUp]
    public new void SetUp()
    {
        base.SetUp();

        FilmRepositoryMock.Setup(r => r.ObtenirParIdAsync(IdFilmValide))
            .ReturnsAsync(Mock.Of<IFilm>(cf => cf.Id == IdFilmValide));

        UtilisateurRepositoryMock.Setup(r => r.ObtenirParIdAsync(IdUtilisateurValide))
            .ReturnsAsync(Mock.Of<IUtilisateur>(cf => cf.Id == IdUtilisateurValide));
    }
}