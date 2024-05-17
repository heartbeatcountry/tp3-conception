using CineQuebec.Domain.Entities.Films;

using Tests.Domain.Entities.Abstract;

namespace Tests.Domain.Entities.Films;

internal class NoteFilmTest : EntiteTests<NoteFilm>
{
    private const byte Note = 4;
    private const byte NoteInvalide = 12;
    private static readonly Guid Film1 = Guid.NewGuid();
    private static readonly Guid Utilisateur1 = Guid.NewGuid();

    protected override object?[] ArgsConstructeur =>
    [
        Utilisateur1,
        Film1,
        Note
    ];


    [Test]
    public void SetNoteFilm_WhenGivenInvalidNote_ShouldThrowArgumentOutOfRangeException()
    {
        Assert.That(() => Entite.SetNote(NoteInvalide), Throws.InstanceOf<ArgumentOutOfRangeException>());
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