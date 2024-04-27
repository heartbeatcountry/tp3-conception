using CineQuebec.Domain.Entities.Utilisateurs;

using Tests.Domain.Entities.Abstract;

namespace Tests.Domain.Entities.Utilisateurs;

public class UtilisateurTests : EntiteTests<Utilisateur>
{
    private const string PrenomValide = "Nouha";
    private const string NomValide = "Bouteldja";
    private const string AutrePrenomValide = "Ginette";
    private const string AutreNomValide = "Bergeron";
    private const string CourrielValide = "nbouteldja@cegepgarneau.ca";
    private const string AutreCourrielValide = "ginette@b.com";
    private const string HashMotDePasseValide = "hashMotDePasseValide";
    private static readonly Role[] RolesValides = [Role.Utilisateur];

    protected override Utilisateur Entite { get; set; } = null!;

    protected override object?[] ArgsConstructeur =>
        [PrenomValide, NomValide, CourrielValide, HashMotDePasseValide, RolesValides];

    [Test]
    public void Constructor_WhenGivenNullOrWhitespacePrenom_ShouldThrowArgumentException()
    {
        // Act
        var exception = Assert.Throws<ArgumentException>(() => CreateInstance(String.Empty, NomValide, CourrielValide,
            HashMotDePasseValide, RolesValides));

        // Assert
        Assert.That(exception.Message, Does.Contain("prénom ne peut pas être vide"));
    }

    [Test]
    public void Constructor_WhenGivenNullOrWhitespaceNom_ShouldThrowArgumentException()
    {
        // Act
        var exception = Assert.Throws<ArgumentException>(() => CreateInstance(PrenomValide, String.Empty, CourrielValide,
            HashMotDePasseValide, RolesValides));

        // Assert
        Assert.That(exception.Message, Does.Contain("nom ne peut pas être vide"));
    }

    [Test]
    public void Constructor_WhenGivenNullOrWhitespaceCourriel_ShouldThrowArgumentException()
    {
        // Act
        var exception = Assert.Throws<ArgumentException>(() => CreateInstance(PrenomValide, NomValide, String.Empty,
            HashMotDePasseValide, RolesValides));

        // Assert
        Assert.That(exception.Message, Does.Contain("courriel ne doit pas être vide"));
    }

    [Test]
    public void Constructor_WhenGivenNullOrWhitespaceHashMotDePasse_ShouldThrowArgumentException()
    {
        // Act
        var exception = Assert.Throws<ArgumentException>(() => CreateInstance(PrenomValide, NomValide, CourrielValide,
            String.Empty, RolesValides));

        // Assert
        Assert.That(exception.Message, Does.Contain("mot de passe ne doit pas être vide"));
    }

    [Test]
    public void Constructor_WhenGiveNoRoles_ShouldAddUtilisateurRole()
    {
        // Assert
        Assert.That(Entite.Roles, Is.EquivalentTo(new[] {Role.Utilisateur}));
    }

    [Test]
    public void CompareTo_WhenGivenDifferentInstanceWithDifferentNomComplet_ShouldReturnComparisonResult()
    {
        // Arrange
        Utilisateur autre = CreateInstance(AutrePrenomValide, AutreNomValide, CourrielValide, HashMotDePasseValide,
            RolesValides);

        // Act & Assert
        Assert.Multiple(() =>
        {
            Assert.That(Entite.CompareTo(autre), Is.GreaterThan(0));
            Assert.That(autre.CompareTo(Entite), Is.LessThan(0));
        });
    }

    [Test]
    public void CompareTo_WhenGivenDifferentInstanceWithSameNomComplet_ShouldReturn0()
    {
        // Arrange
        Utilisateur autre = CreateInstance(PrenomValide, NomValide, CourrielValide, HashMotDePasseValide, RolesValides);
        autre.SetId(Guid.NewGuid());

        // Act
        int result = Entite.CompareTo(autre);

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void CompareTo_WhenGivenNull_ShouldReturn1()
    {
        // Act
        int result = Entite.CompareTo(null);

        // Assert
        Assert.That(result, Is.EqualTo(1));
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
    public void Equals_WhenGivenOtherInstanceWithDifferentCourrielAndId_ShouldReturnFalse()
    {
        // Arrange
        Utilisateur autre = CreateInstance(PrenomValide, NomValide, AutreCourrielValide, HashMotDePasseValide, RolesValides);
        autre.SetId(Guid.NewGuid());

        // Assert
        Assert.That(Entite.Equals(autre), Is.False);
    }

    [Test]
    public void Equals_WhenGivenOtherInstanceWithSameId_ShouldReturnTrue()
    {
        // Arrange
        Entite.SetId(Guid.NewGuid());
        Utilisateur autre = CreateInstance(PrenomValide, NomValide, CourrielValide, HashMotDePasseValide, RolesValides);
        autre.SetId(Entite.Id);

        // Assert
        Assert.That(Entite.Equals(autre), Is.True);
    }

    [Test]
    public void Equals_WhenGivenOtherInstanceWithSameCourriel_ShouldReturnTrue()
    {
        // Arrange
        Utilisateur autre = CreateInstance(PrenomValide, NomValide, CourrielValide, HashMotDePasseValide, RolesValides);

        // Assert
        Assert.That(Entite.Equals(autre), Is.True);
    }

    [Test]
    public void NomComplet_Always_ShouldBeEqualToPrenomAndNom()
    {
        Assert.That(Entite.NomComplet, Is.EqualTo($"{PrenomValide} {NomValide}"));
    }

    [Test]
    public override void ToString_Always_ShouldUniquelyDescribeTheEntity()
    {
        // Assert
        Assert.That(Entite.ToString(), Is.EqualTo($"{PrenomValide} {NomValide}"));
    }

    [Test]
    public void SetHashMotDePasse_WhenGivenNullOrWhitespaceHashMotDePasse_ShouldThrowArgumentException()
    {
        Assert.Multiple(() =>
        {
            Assert.That(() => Entite.SetHashMotDePasse(string.Empty), Throws.ArgumentException);
            Assert.That(() => Entite.SetHashMotDePasse(" "), Throws.ArgumentException);
        });
    }

    [Test]
    public void SetHashMotDePasse_WhenGivenValidHashMotDePasse_ShouldSetHashMotDePasseToGivenHashMotDePasse()
    {
        // Act
        Entite.SetHashMotDePasse("newHashMotDePasse");

        // Assert
        Assert.That(Entite.HashMotDePasse, Is.EqualTo("newHashMotDePasse"));
    }

    [Test]
    public void AddActeursFavoris_WhenGivenActeurNotPresentInActeursFavoris_ShouldAddActeurToActeursFavoris()
    {
        // Arrange
        Guid acteur = Guid.NewGuid();

        // Act
        Entite.AddActeursFavoris([acteur]);

        // Assert
        Assert.That(Entite.ActeursFavorisParId, Contains.Item(acteur));
    }

    [Test]
    public void AddActeursFavoris_WhenTryingToAddMoreThanMaxActeursFavoris_ShouldThrowArgumentException()
    {
        // Arrange
        Entite.AddActeursFavoris(Enumerable.Range(0, Utilisateur.MaxActeursFavoris)
            .Select(_ => Guid.NewGuid()));

        // Act & Assert
        Assert.That(() => Entite.AddActeursFavoris([Guid.NewGuid()]), Throws.ArgumentException);
    }

    [Test]
    public void AddActeursFavoris_WhenGivenActeurPresentInActeursFavoris_ShouldNotAddActeurToActeursFavoris()
    {
        // Arrange
        Guid acteur = Guid.NewGuid();
        Entite.AddActeursFavoris([acteur]);

        // Act
        Entite.AddActeursFavoris([acteur]);

        // Assert
        Assert.That(Entite.ActeursFavorisParId.Count, Is.EqualTo(1));
    }

    [Test]
    public void AddRealisateursFavoris_WhenGivenRealisateurNotPresentInRealisateursFavoris_ShouldAddRealisateurToRealisateursFavoris()
    {
        // Arrange
        Guid realisateur = Guid.NewGuid();

        // Act
        Entite.AddRealisateursFavoris([realisateur]);

        // Assert
        Assert.That(Entite.RealisateursFavorisParId, Contains.Item(realisateur));
    }

    [Test]
    public void AddRealisateursFavoris_WhenTryingToAddMoreThanMaxRealisateursFavoris_ShouldThrowArgumentException()
    {
        // Arrange
        Entite.AddRealisateursFavoris(Enumerable.Range(0, Utilisateur.MaxRealisateursFavoris)
            .Select(_ => Guid.NewGuid()));

        // Act & Assert
        Assert.That(() => Entite.AddRealisateursFavoris([Guid.NewGuid()]), Throws.ArgumentException);
    }

    [Test]
    public void AddRealisateursFavoris_WhenGivenRealisateurPresentInRealisateursFavoris_ShouldNotAddRealisateurToRealisateursFavoris()
    {
        // Arrange
        Guid realisateur = Guid.NewGuid();
        Entite.AddRealisateursFavoris([realisateur]);

        // Act
        Entite.AddRealisateursFavoris([realisateur]);

        // Assert
        Assert.That(Entite.RealisateursFavorisParId.Count, Is.EqualTo(1));
    }

    [Test]
    public void AddBillets_WhenGivenBilletNotPresentInBillets_ShouldAddBilletToBillets()
    {
        // Arrange
        Guid billet = Guid.NewGuid();

        // Act
        Entite.AddBillets([billet]);

        // Assert
        Assert.That(Entite.BilletsParId, Contains.Item(billet));
    }

    [Test]
    public void AddBillets_WhenGivenBilletPresentInBillets_ShouldNotAddBilletToBillets()
    {
        // Arrange
        Guid billet = Guid.NewGuid();
        Entite.AddBillets([billet]);

        // Act
        Entite.AddBillets([billet]);

        // Assert
        Assert.That(Entite.BilletsParId.Count, Is.EqualTo(1));
    }

    [Test]
    public void AddCategoriesPreferees_WhenGivenCategorieNotPresentInCategoriesPreferees_ShouldAddCategorieToCategoriesPreferees()
    {
        // Arrange
        Guid categorie = Guid.NewGuid();

        // Act
        Entite.AddCategoriesPreferees([categorie]);

        // Assert
        Assert.That(Entite.CategoriesPrefereesParId, Contains.Item(categorie));
    }

    [Test]
    public void AddCategoriesPreferees_WhenTryingToAddMoreThanMaxCategoriesPreferees_ShouldThrowArgumentException()
    {
        // Arrange
        Entite.AddCategoriesPreferees(Enumerable.Range(0, Utilisateur.MaxCategoriesPreferees)
            .Select(_ => Guid.NewGuid()));

        // Act & Assert
        Assert.That(() => Entite.AddCategoriesPreferees([Guid.NewGuid()]), Throws.ArgumentException);
    }

    [Test]
    public void AddCategoriesPreferees_WhenGivenCategoriePresentInCategoriesPreferees_ShouldNotAddCategorieToCategoriesPreferees()
    {
        // Arrange
        Guid categorie = Guid.NewGuid();
        Entite.AddCategoriesPreferees([categorie]);

        // Act
        Entite.AddCategoriesPreferees([categorie]);

        // Assert
        Assert.That(Entite.CategoriesPrefereesParId.Count, Is.EqualTo(1));
    }
}