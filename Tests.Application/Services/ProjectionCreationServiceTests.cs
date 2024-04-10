using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using CineQuebec.Application.Services;
using CineQuebec.Domain.Entities.Abstract;
using CineQuebec.Domain.Entities.Films;
using CineQuebec.Domain.Entities.Projections;
using CineQuebec.Domain.Interfaces.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Projections;

using Moq;

namespace Tests.Application.Services
{
    public class ProjectionCreationServiceTests : GenericServiceTests<ProjectionCreationService>
    {
        private static readonly Guid _idFilmValide = Guid.NewGuid();
        private static readonly Guid _idSalleValide = Guid.NewGuid();
        private static readonly DateTime DateValide = new(2024, 12, 19);
        private static readonly DateTime DateInvalide = new(2022, 11, 08);
        private static readonly Boolean estAvantPremiere = true;
        private static readonly Boolean pasAvantPremiere = false;

        protected override void SetUpExt()
        {
            FilmRepositoryMock.Setup(r => r.ObtenirParIdAsync(_idFilmValide))
                .ReturnsAsync(Mock.Of<IFilm>(cf => cf.Id == _idFilmValide));
            
            SalleRepositoryMock.Setup(r => r.ObtenirParIdAsync(_idSalleValide))
                .ReturnsAsync(Mock.Of<ISalle>(cf => cf.Id == _idSalleValide));

        }


        [Test]
        public void CreerProjection_WhenGivenInvalidFilm_ThrowsAggregateExceptionContainingArgumentException()
        {
            //Arrange
            FilmRepositoryMock.Setup(r => r.ObtenirParIdAsync(_idFilmValide))
               .ReturnsAsync((Film?)null);


            // Act & Assert
            AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
                Service.CreerProjection(_idFilmValide, _idSalleValide, DateValide, estAvantPremiere));
            Assert.That(aggregateException?.InnerExceptions,
                Has.One.InstanceOf<ArgumentException>().With.Message.Contains("doit durer plus de 0 minutes"));
        }


        [Test]
        public void CreerProjection_WhenGivenInvalidSalle_ThrowsAggregateExceptionContainingArgumentException()
        {
            //Arrange
            SalleRepositoryMock.Setup(r => r.ObtenirParIdAsync(_idSalleValide))
               .ReturnsAsync((Salle?)null);

            // Act & Assert
            AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
                Service.CreerProjection(_idFilmValide, _idSalleValide, DateValide, estAvantPremiere));
            Assert.That(aggregateException?.InnerExceptions,
                Has.One.InstanceOf<ArgumentException>().With.Message.Contains("n'existe pas"));
        }


        [Test]
        public void CreerProjection_WhenGivenIdFilmAndIdSalleAndDateHeureAlreadyPrensentInRepository_ThrowsAggregateExceptionContainingArgumentException()
        {
            //Arrange
           
            FilmRepositoryMock.Setup(r => r.ExisteAsync(It.IsAny<Expression<Func<IFilm, bool>>>()))
                .ReturnsAsync(true);
            SalleRepositoryMock.Setup(r => r.ExisteAsync(It.IsAny<Expression<Func<ISalle, bool>>>()))
                .ReturnsAsync(true);

            // Act & Assert
            AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
                Service.CreerProjection(_idFilmValide, _idSalleValide, DateValide, estAvantPremiere));
            Assert.That(aggregateException?.InnerExceptions,
                Has.One.InstanceOf<ArgumentException>().With.Message.Contains("existe déjà"));
        }


        [Test]
        public void CreerProjection_WhenGivenIdSalleAndDateHeureAlreadyPrensentInRepository_ThrowsAggregateExceptionContainingArgumentException()
        {
            //Arrange

            SalleRepositoryMock.Setup(r => r.ExisteAsync(It.IsAny<Expression<Func<ISalle, bool>>>()))
                .ReturnsAsync(true);

            // Act & Assert
            AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
                Service.CreerProjection(_idFilmValide, _idSalleValide, DateValide, estAvantPremiere));
            Assert.That(aggregateException?.InnerExceptions,
                Has.One.InstanceOf<ArgumentException>().With.Message.Contains("existe déjà"));
        }


        [Test]
        public void CreerProjection_WhenGivenInvalidDate_ThrowsAggregateExceptionContainingArgumentException()
        {
            // Act & Assert
            AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
                Service.CreerProjection(_idFilmValide, _idSalleValide, DateInvalide, estAvantPremiere));
            Assert.That(aggregateException?.InnerExceptions,
                Has.One.InstanceOf<ArgumentException>().With.Message
                    .Contains("date de projection doit être supérieure à"));
        }


    }
}