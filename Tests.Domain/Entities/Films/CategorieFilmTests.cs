using CineQuebec.Domain.Entities.Films;
using Tests.Domain.Entities.Abstract;

namespace Tests.Domain.Entities.Films;

public class CategorieFilmTests : EntiteTests<CategorieFilm>
{
	private const string NomValide = "Action";
	private const string AutreNomValide = "Comédie";
	protected override object?[] ArgsConstructeur => [NomValide];

	[Test]
	public override void ToString_Always_ShouldUniquelyDescribeTheEntity()
	{
		// Assert
		Assert.That(Entite.ToString(), Is.EqualTo(Entite.NomAffichage));
	}

	[Test]
	public void AfterInstantiation_NomAffichageShouldBeEqualToNomAffichageGivenInConstructor()
	{
		// Assert
		Assert.That(Entite.NomAffichage, Is.EqualTo(NomValide));
	}

	[Test]
	public void OnInstantiation_ConstructorShouldThrowArgumentExceptionWhenNomAffichageIsNullOrWhitespace()
	{
		Assert.That(() => CreateInstance(string.Empty), Throws.ArgumentException);
		Assert.That(() => CreateInstance(" "), Throws.ArgumentException);
	}

	[Test]
	public void WhenSetNomAffichageWithNullOrWhitespace_ThenThrowArgumentException()
	{
		Assert.That(() => Entite.SetNomAffichage(null), Throws.ArgumentException);
		Assert.That(() => Entite.SetNomAffichage(string.Empty), Throws.ArgumentException);
		Assert.That(() => Entite.SetNomAffichage(" "), Throws.ArgumentException);
	}

	[Test]
	public void WhenSetNomAffichageWithValidValue_ThenNomAffichageShouldBeEqualToValue()
	{
		// Act
		Entite.SetNomAffichage(AutreNomValide);

		// Assert
		Assert.That(Entite.NomAffichage, Is.EqualTo(AutreNomValide));
	}

	[Test]
	public void WhenCompareToWithNull_ThenReturn1()
	{
		// Act
		var result = Entite.CompareTo(null);

		// Assert
		Assert.That(result, Is.EqualTo(1));
	}

	[Test]
	public void WhenCompareToWithSameObject_ThenReturn0()
	{
		// Act
		var result = Entite.CompareTo(Entite);

		// Assert
		Assert.That(result, Is.EqualTo(0));
	}

	[Test]
	public void WhenCompareToWithDifferentObject_ThenReturnComparisonResult()
	{
		// Arrange
		var autreCategorie = new CategorieFilm(AutreNomValide);

		// Act
		var result = Entite.CompareTo(autreCategorie);

		// Assert
		Assert.That(result,
			Is.EqualTo(string.Compare(Entite.NomAffichage, autreCategorie.NomAffichage, StringComparison.Ordinal)));
	}
}