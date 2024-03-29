using CineQuebec.Domain.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Films;
using Moq;
using Tests.Domain.Entities.Abstract;

namespace Tests.Domain.Entities.Films;

public class FilmTests : EntiteTests<Film>
{
	private const string TitreValide = "Le Seigneur des Anneaux";
	private const string AutreTitreValide = "Trouver Nemo";
	private const string DescriptionValide = "Un film de Peter Jackson";
	private const ushort DureeValide = 178;

	private static readonly IActeur Acteur1 = Mock.Of<IActeur>(a => a.Id == Guid.NewGuid() && a.Prenom == "Michel" && a
		.Nom == "Sardou" && a.NomComplet == "Michel Sardou");

	private static readonly IActeur Acteur2 = Mock.Of<IActeur>(a => a.Id == Guid.NewGuid() && a.Prenom == "Ginette" && a
		.Nom == "Bergeron" && a.NomComplet == "Ginette Bergeron");

	private static readonly IRealisateur Realisateur1 = Mock.Of<IRealisateur>(r => r.Id == Guid.NewGuid() &&
		r.Prenom == "Peter" && r.Nom == "Jackson" && r.NomComplet == "Peter Jackson");

	private static readonly IRealisateur Realisateur2 = Mock.Of<IRealisateur>(r => r.Id == Guid.NewGuid() &&
		r.Prenom == "Andrew" && r.Nom == "Stanton" && r.NomComplet == "Andrew Stanton");

	private static readonly IEnumerable<IActeur> AucunActeurs = Array.Empty<IActeur>();
	private static readonly IEnumerable<IActeur> DeuxActeurs = new[] { Acteur1, Acteur2 };
	private static readonly IEnumerable<IRealisateur> AucunRealisateurs = Array.Empty<IRealisateur>();
	private static readonly IEnumerable<IRealisateur> DeuxRealisateurs = new[] { Realisateur1, Realisateur2 };
	private static readonly ICategorieFilm CategorieFilm = Mock.Of<ICategorieFilm>(cf => cf.NomAffichage == "Action");
	private static readonly DateOnly DateSortieInternationale = new(2001, 12, 19);

	protected override object?[] ArgsConstructeur =>
	[
		TitreValide, DescriptionValide, CategorieFilm, DateSortieInternationale, DeuxActeurs,
		DeuxRealisateurs, DureeValide,
	];

	[Test]
	public void CompareTo_WhenGivenNull_ShouldReturn1()
	{
		// Act
		var result = Entite.CompareTo(null);

		// Assert
		Assert.That(result, Is.EqualTo(1));
	}

	[Test]
	public void CompareTo_WhenGivenOtherInstanceWithDifferentTitre_ShouldReturnComparisonResult()
	{
		// Arrange
		var autre = CreateInstance();
		autre.SetTitre(AutreTitreValide);

		// Act
		var result = Entite.CompareTo(autre);

		// Assert
		Assert.That(result,
			Is.EqualTo(string.Compare(Entite.Titre, autre.Titre, StringComparison.Ordinal)));
	}

	[Test]
	public void CompareTo_WhenGivenSameInstance_ShouldReturn0()
	{
		// Act
		var result = Entite.CompareTo(Entite);

		// Assert
		Assert.That(result, Is.EqualTo(0));
	}

	[Test]
	public void Constructor_WhenGivenActeurs_ShouldAddGivenActeursToActeurs()
	{
		// Assert
		Assert.That(Entite.Acteurs, Is.SupersetOf(AucunActeurs));
	}

	[Test]
	public void Constructor_WhenGivenNullCategorie_ShouldThrowArgumentNullException()
	{
		Assert.That(() => CreateInstance(TitreValide, DescriptionValide, null, DateSortieInternationale,
			AucunActeurs, AucunRealisateurs, DureeValide), Throws.ArgumentNullException);
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
		Assert.That(Entite.Realisateurs, Is.SupersetOf(DeuxRealisateurs));
	}

	[Test]
	public void Constructor_WhenGivenValidCategorieFilm_ShouldSetCategorieFilm()
	{
		// Assert
		Assert.That(Entite.Categorie, Is.EqualTo(CategorieFilm));
	}

	[Test]
	public void Constructor_WhenGivenValidDateSortie_ShouldSetDateSortie()
	{
		// Assert
		Assert.That(Entite.DateSortieInternationale, Is.EqualTo(DateSortieInternationale));
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
		var autre = CreateInstance();
		autre.SetTitre(AutreTitreValide);
		autre.SetId(Guid.NewGuid());

		// Assert
		Assert.That(Entite.Equals(autre), Is.False);
	}

	[Test]
	public void Equals_WhenGivenOtherInstanceWithSameTitreAndDateSortieAndDuree_ShouldReturnTrue()
	{
		// Arrange
		var autre = CreateInstance();
		autre.SetId(Guid.NewGuid());

		// Assert
		Assert.That(Entite.Equals(autre), Is.True);
	}

	[Test]
	public void
		SetDateSortieInternationale_WhenGivenValidDateSortie_ShouldSetDateSortieInternationaleToGivenDateSortie()
	{
		// Arrange
		var nouvelleDateSortie = new DateOnly(2003, 12, 25);

		// Act
		Entite.SetDateSortieInternationale(nouvelleDateSortie);

		// Assert
		Assert.That(Entite.DateSortieInternationale, Is.EqualTo(nouvelleDateSortie));
	}

	[Test]
	public void SetDateSortieInternationale_WhenGivenZeroDate_ShouldThrowArgumentOutOfRangeException()
	{
		Assert.That(() => Entite.SetDateSortieInternationale(DateOnly.MinValue), Throws
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