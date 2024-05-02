using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using CineQuebec.Application.Services;
using CineQuebec.Domain.Entities.Films;
using CineQuebec.Domain.Entities.Utilisateurs;
using CineQuebec.Domain.Entities.Projections;
using CineQuebec.Domain.Interfaces.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Projections;
using Moq;

namespace Tests.Application.Services
{
    public class NoteFilmCreationServiceTests : GenericServiceTests<NoteFilmCreationService>
    {


        private static readonly Guid IdFilmValide = Guid.NewGuid();
        private static readonly Guid IdUtilisateurValide = Guid.NewGuid();
        private static readonly ushort NoteValide = 7;
        private static readonly ushort NoteInvalide = 14;


        [Test]
        public void CreerNoteFilm_WhenGivenInvalidFilm_ThrowsAggregateExceptionContainingArgumentException()
        {
            //Arrange
            FilmRepositoryMock.Setup(r => r.ObtenirParIdAsync(IdFilmValide))
                .ReturnsAsync((Film?)null);

            // Act & Assert
            AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
                Service.NoterFilm(IdFilmValide, IdUtilisateurValide, NoteValide));
            Assert.That(aggregateException?.InnerExceptions,
                Has.One.InstanceOf<ArgumentException>().With.Message.Contains("n'existe pas"));
        }


        [Test]
        public void CreerNoteFilm_WhenGivenInvalIdUtilisateur_ThrowsAggregateExceptionContainingArgumentException()
        {
            //Arrange
            UtilisateurRepositoryMock.Setup(r => r.ObtenirParIdAsync(IdUtilisateurValide))
                .ReturnsAsync((Utilisateur?)null);

            // Act & Assert
            AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
                  Service.NoterFilm(IdFilmValide, IdUtilisateurValide, NoteValide));
            Assert.That(aggregateException?.InnerExceptions,
                Has.One.InstanceOf<ArgumentException>().With.Message.Contains("n'existe pas"));
        }



        [Test]
        public void
            CreerNoteFilm_WhenGivenIdFilmAndIdUtilisateurAlreadyPrensentInRepository_ThrowsAggregateExceptionContainingArgumentException()
        {
            //Arrange
            NoteFilmRepositoryMock.Setup(r => r.ExisteAsync(It.IsAny<Expression<Func<INoteFilm, bool>>>()))
                .ReturnsAsync(true);

            // Act & Assert
            AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
                Service.NoterFilm(IdFilmValide, IdUtilisateurValide, NoteValide));
            Assert.That(aggregateException?.InnerExceptions,
                Has.One.InstanceOf<ArgumentException>().With.Message.Contains("existe déjà"));
        }


        [Test]
        public void CreerNoteFilm_WhenGivenInvalidNote_ThrowsAggregateExceptionContainingArgumentException()
        {
            // Act & Assert
            AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
                Service.NoterFilm(IdFilmValide, IdUtilisateurValide, NoteInvalide));
            Assert.That(aggregateException?.InnerExceptions,
                Has.One.InstanceOf<ArgumentException>().With.Message
                    .Contains("La noteFilm doit être entre 0 et 10."));
        }

        //TODO
        //[Test]
        //public async Task CreerNoteFilm_WhenGivenValidArguments_ShouldAddNoteFilmToRepository()
        //{
        //    //Arrange
        //    NoteFilmRepositoryMock.Setup(r => r.ExisteAsync(It.IsAny<Expression<Func<INoteFilm, bool>>>()))
        //        .ReturnsAsync(false);
        //    FilmRepositoryMock.Setup(r => r.ObtenirParIdAsync(IdFilmValide))
        //        .ReturnsAsync(Mock.Of<IFilm>(cf => cf.Id == IdFilmValide));
        //    UtilisateurRepositoryMock.Setup(r => r.ObtenirParIdAsync(IdUtilisateurValide))
        //        .ReturnsAsync(Mock.Of<IUtilisateur>(cf => cf.Id == IdUtilisateurValide));
        //    NoteFilmRepositoryMock.Setup(r => r.AjouterAsync(It.IsAny<NoteFilm>()))
        //        .ReturnsAsync(Mock.Of<INoteFilm>(a => a.Id == Guid.NewGuid()));

        //    // Act
        //    await Service.CreerNoteFilm(IdFilmValide, IdUtilisateurValide, NoteInvalide));

        //    // Assert
        //    NoteFilmRepositoryMock.Verify(r => r.AjouterAsync(It.IsAny<NoteFilm>()));
        //}

        //[Test]
        //public async Task CreerNoteFilm_WhenGivenValidArguments_ShouldReturnGuidOfNewNoteFilm()
        //{
        //    //Arrange

        //    FilmRepositoryMock.Setup(r => r.ObtenirParIdAsync(IdFilmValide))
        //        .ReturnsAsync(Mock.Of<IFilm>(cf => cf.Id == IdFilmValide));
        //    UtilisateurRepositoryMock.Setup(r => r.ObtenirParIdAsync(IdUtilisateurValide))
        //        .ReturnsAsync(Mock.Of<IUtilisateur>(cf => cf.Id == IdUtilisateurValide));


        //    // Act
        //    Guid noteFilmId = await Service.CreerNoteFilm(IdFilmValide, IdUtilisateurValide, NoteValide);

        //    // Assert
        //    Assert.That(noteFilmId, Is.Not.EqualTo(Guid.Empty));
        //}

        protected override void SetUpExt()
        {
            FilmRepositoryMock.Setup(r => r.ObtenirParIdAsync(IdFilmValide))
                .ReturnsAsync(Mock.Of<IFilm>(cf => cf.Id == IdFilmValide));

            //UtilisateurRepositoryMock.Setup(r => r.ObtenirParIdAsync(IdUtilisateurValide))
            //    .ReturnsAsync(Mock.Of<IUtilisateur>(cf => cf.Id == IdUtilisateurValide));
        }
    }
}