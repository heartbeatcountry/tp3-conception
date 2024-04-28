﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CineQuebec.Domain.Entities.Films;

using Tests.Domain.Entities.Abstract;

namespace Tests.Domain.Entities.Films
{
    internal class NoteFilmTest : EntiteTests<NoteFilm>
    {


        private static readonly Guid Film1 = Guid.NewGuid();
        private static readonly Guid Utilisateur1 = Guid.NewGuid();
        private static readonly ushort Note = 4;
        private static readonly ushort NoteInvalide = 12;

        protected override object?[] ArgsConstructeur =>
  [
      Film1,
      Utilisateur1,
      Note
  ];


        [Test]
        public void SetNoteFilm_WhenGivenInvalidNote_ShouldThrowArgumentOutOfRangeException()
        {
            Assert.That(() => Entite.SetNoteFilm(NoteInvalide), Throws
                .InstanceOf<ArgumentOutOfRangeException>());
        }


        [Test]
        public void Constructor_WhenGivenValidFilm_ShouldSetFilm()
        {
            // Assert
            Assert.That(Entite.IdFilm, Is.EqualTo(Film1));
        }  
        
        
        [Test]
        public void Constructor_WhenGivenValidUtilisateur_ShouldSetUtilisateur()
        {
            // Assert
            Assert.That(Entite.IdUtilisateur, Is.EqualTo(Utilisateur1));
        }


        [Test]
        public void Constructor_WhenGivenValidNote_ShouldSetNote()
        {
            // Assert
            Assert.That(Entite.Note, Is.EqualTo(Note));
        }

    }
}