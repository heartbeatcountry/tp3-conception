using CineQuebec.Domain.Entities.Films;

using Tests.Domain.Entities.Abstract;

namespace Tests.Domain.Entities.Films;

public class FilmTests : EntiteTests<Film>
{
    private const string TitreValide = "Le Seigneur des Anneaux";
    private const string AutreTitreValide = "Trouver Nemo";
    private const string DescriptionValide = "Un film de Peter Jackson";
    private const ushort DureeValide = 178;

    private static readonly Guid Acteur1 = Guid.NewGuid();
    private static readonly Guid Acteur2 = Guid.NewGuid();
    private static readonly Guid Realisateur1 = Guid.NewGuid();
    private static readonly Guid Realisateur2 = Guid.NewGuid();
    private static readonly IEnumerable<Guid> AucunActeurs = Array.Empty<Guid>();
    private static readonly IEnumerable<Guid> DeuxActeurs = new[] { Acteur1, Acteur2 };
    private static readonly IEnumerable<Guid> AucunRealisateurs = Array.Empty<Guid>();
    private static readonly IEnumerable<Guid> DeuxRealisateurs = new[] { Realisateur1, Realisateur2 };
    private static readonly Guid CategorieFilm = Guid.NewGuid();
    private static readonly DateTime DateSortieInternationale = new(2001, 12, 19);

    protected override object?[] ArgsConstructeur =>
    [
        TitreValide, DescriptionValide, CategorieFilm, DateSortieInternationale, DeuxActeurs,
        DeuxRealisateurs, DureeValide
    ];

    [Test]
    public void CompareTo_WhenGivenNull_ShouldReturn1()
    {
        // Act
        int result = Entite.CompareTo(null);

        // Assert
        Assert.That(result, Is.EqualTo(1));
    }

    [Test]
    public void CompareTo_WhenGivenOtherInstanceWithDifferentTitre_ShouldReturnComparisonResult()
    {
        // Arrange
        Film autre = CreateInstance();
        autre.SetTitre(AutreTitreValide);

        // Act
        int result = Entite.CompareTo(autre);

        // Assert
        Assert.That(result,
            Is.EqualTo(string.Compare(Entite.Titre, autre.Titre, StringComparison.Ordinal)));
    }

    [Test]
    public void CompareTo_WhenGivenSameInstance_ShouldReturn0()
    {
        // Act
        int result = Entite.CompareTo(Entite);

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void Constructor_WhenGivenActeurs_ShouldAddGivenActeursToActeurs()
    {
        // Assert
        Assert.That(Entite.ActeursParId, Is.SupersetOf(AucunActeurs));
    }

    [Test]
    public void Constructor_WhenGivenNullCategorie_ShouldThrowArgumentException()
    {
        Assert.That(() => CreateInstance(TitreValide, DescriptionValide, Guid.Empty, DateSortieInternationale,
            AucunActeurs, AucunRealisateurs, DureeValide), Throws.ArgumentException);
    }

    [Test]
    public void Constructor_WhenGivenNullOrWhitespaceDescription_ShouldThrowArgumentException()
    {
        Assert.Multiple(() =>
        {
            Assert.That(() => CreateInstance(TitreValide, string.Empty, CategorieFilm, DateSortieInternationale,
                AucunActeurs, AucunRealisateurs, DureeValide), Throws.ArgumentException);
            Assert.That(() => CreateInstance(TitreValide, " ", CategorieFilm, DateSortieInternationale,
                AucunActeurs, AucunRealisateurs, DureeValide), Throws.ArgumentException);
        });
    }

    [Test]
    public void Constructor_WhenGivenNullOrWhitespaceTitre_ShouldThrowArgumentException()
    {
        Assert.Multiple(() =>
        {
            Assert.That(() => CreateInstance(string.Empty, DescriptionValide, CategorieFilm, DateSortieInternationale,
                AucunActeurs, AucunRealisateurs, DureeValide), Throws.ArgumentException);
            Assert.That(() => CreateInstance(" ", DescriptionValide, CategorieFilm, DateSortieInternationale,
                AucunActeurs, AucunRealisateurs, DureeValide), Throws.ArgumentException);
        });
    }

    [Test]
    public void Constructor_WhenGivenRealisateurs_ShouldAddGivenRealisateursToRealisateurs()
    {
        // Assert
        Assert.That(Entite.RealisateursParId, Is.SupersetOf(DeuxRealisateurs));
    }

    [Test]
    public void Constructor_WhenGivenValidCategorieFilm_ShouldSetCategorieFilm()
    {
        // Assert
        Assert.That(Entite.IdCategorie, Is.EqualTo(CategorieFilm));
    }

    [Test]
    public void Constructor_WhenGivenValidDateSortie_ShouldSetDateSortie()
    {
        // Assert
#pragma warning disable NUnit2021
        Assert.That(Entite.DateSortieInternationale, Is.EqualTo(DateSortieInternationale));
#pragma warning restore NUnit2021
    }

    [Test]
    public void Constructor_WhenGivenValidDescription_ShouldSetDescription()
    {
        // Assert
        Assert.That(Entite.Description, Is.EqualTo(DescriptionValide));
    }

    [Test]
    public void Constructor_WhenGivenValidDuree_ShouldSetDuree()
    {
        // Assert
        Assert.That(Entite.DureeEnMinutes, Is.EqualTo(DureeValide));
    }

    [Test]
    public void Constructor_WhenGivenValidTitre_ShouldSetTitre()
    {
        // Assert
        Assert.That(Entite.Titre, Is.EqualTo(TitreValide));
    }

    [Test]
    public void Equals_WhenGivenOtherInstanceWithDifferentTitreAndDateSortieAndDureeAndId_ShouldReturnFalse()
    {
        // Arrange
        Film autre = CreateInstance();
        autre.SetTitre(AutreTitreValide);
        autre.SetId(Guid.NewGuid());

        // Assert
        Assert.That(Entite.Equals(autre), Is.False);
    }

    [Test]
    public void Equals_WhenGivenOtherInstanceWithSameTitreAndDateSortieAndDuree_ShouldReturnTrue()
    {
        // Arrange
        Film autre = CreateInstance();
        autre.SetId(Guid.NewGuid());

        // Assert
        Assert.That(Entite.Equals(autre), Is.True);
    }

    [Test]
    public void
        SetDateSortieInternationale_WhenGivenValidDateSortie_ShouldSetDateSortieInternationaleToGivenDateSortie()
    {
        // Arrange
        DateTime nouvelleDateSortie = new(2022, 12, 19);

        // Act
        Entite.SetDateSortieInternationale(nouvelleDateSortie);

        // Assert
#pragma warning disable NUnit2021
        Assert.That(Entite.DateSortieInternationale, Is.EqualTo(nouvelleDateSortie));
#pragma warning restore NUnit2021
    }

    [Test]
    public void SetDateSortieInternationale_WhenGivenZeroDate_ShouldThrowArgumentOutOfRangeException()
    {
        Assert.That(() => Entite.SetDateSortieInternationale(DateTime.MinValue), Throws
            .InstanceOf<ArgumentOutOfRangeException>());
    }

    [Test]
    public void SetDureeEnMinutes_WhenGivenValidDuree_ShouldSetDureeEnMinutesToGivenDuree()
    {
        // Arrange
        const ushort nouvelleDuree = 120;

        // Act
        Entite.SetDureeEnMinutes(nouvelleDuree);

        // Assert
        Assert.That(Entite.DureeEnMinutes, Is.EqualTo(nouvelleDuree));
    }

    [Test]
    public void SetDureeEnMinutes_WhenGivenZeroDuree_ShouldThrowArgumentNullException()
    {
        Assert.That(() => Entite.SetDureeEnMinutes(0), Throws.InstanceOf<ArgumentOutOfRangeException>());
    }

    [Test]
    public override void ToString_Always_ShouldUniquelyDescribeTheEntity()
    {
        // Assert
        Assert.That(Entite.ToString(), Is.EqualTo($"{Entite.Titre} ({Entite.DateSortieInternationale.Year})"));
    }
}