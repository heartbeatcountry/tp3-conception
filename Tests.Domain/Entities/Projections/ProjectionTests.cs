using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CineQuebec.Domain.Entities.Films;
using CineQuebec.Domain.Entities.Projections;
using CineQuebec.Domain.Interfaces.Entities.Projections;

using Tests.Domain.Entities.Abstract;

namespace Tests.Domain.Entities.Projections
{
    public class ProjectionTests : EntiteTests<Projection>
    {
        private static readonly Guid Film1= Guid.NewGuid();
        private static readonly Guid Film2 = Guid.NewGuid();
        private static readonly Guid Salle1 = Guid.NewGuid();
        private static readonly Guid Salle2 = Guid.NewGuid();
        private static readonly DateTime DateValide = new(2024, 12, 19);
        private static readonly DateTime DateInvalide = new(2022, 11, 08);
        private static readonly Boolean estAvantPremiere = true;
        private static readonly Boolean pasAvantPremiere = false;

        protected override object?[] ArgsConstructeur =>
    [
        Film1,
        Salle1,
        DateValide,
        estAvantPremiere
    ];

        [Test]
        public void Equals_WhenGivenOtherInstanceWithSameFilmAndSalleAndDateHeure_ShouldReturnTrue()
        {
            // Arrange
            var autre = CreateInstance();
            autre.SetId(Guid.NewGuid());

            // Assert
            Assert.That(Entite.Equals(autre), Is.True);
        }

        [Test]
        public void SetDateHeure_WhenGivenZeroDate_ShouldThrowArgumentOutOfRangeException()
        {
            Assert.That(() => Entite.SetDateHeure(DateTime.MinValue), Throws
                .InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void Constructor_WhenGivenValidEstAvantPremiere_ShouldSetEstAvantPremiere()
        {
            // Assert
            Assert.That(() => Entite.EstAvantPremiere, Is.True);
        }


        [Test]
        public void Constructor_WhenGivenValidFilm_ShouldSetFilm()
        {
            // Assert
            Assert.That(Entite.IdFilm, Is.EqualTo(Film1));
        }


        [Test]
        public void Constructor_WhenGivenValidSalle_ShouldSetSalle()
        {
            // Assert
            Assert.That(Entite.IdSalle, Is.EqualTo(Salle1));
        }





    }
}
