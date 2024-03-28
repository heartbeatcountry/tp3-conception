using CineQuebec.Domain.Entities.Abstract;

namespace Tests.Domain.Entities.Abstract;

public abstract class PersonneTests<T> : EntiteTests<T> where T : Personne
{
	private const string PrenomValide = "Michel";
	private const string NomValide = "Sardou";
	private const string AutreNomValide = "Bergeron";
	protected override object?[] ArgsConstructeur => [PrenomValide, NomValide];

	[Test]
	public override void ToString_Always_ShouldUniquelyDescribeTheEntity()
	{
		// Assert
		Assert.That(Entite.ToString(), Is.EqualTo($"{PrenomValide} {NomValide}"));
	}

	[Test]
	public void Constructor_Always_ShouldSetPrenomToPrenomGivenInConstructor()
	{
		// Assert
		Assert.That(Entite.Prenom, Is.EqualTo(PrenomValide));
	}

	[Test]
	public void Constructor_Always_ShouldSetNomToNomGivenInConstructor()
	{
		// Assert
		Assert.That(Entite.Nom, Is.EqualTo(NomValide));
	}

	[Test]
	public void Constructor_WhenGivenNullOrWhitespacePrenom_ShouldThrowArgumentException()
	{
		Assert.That(() => CreateInstance(null, NomValide), Throws.ArgumentException);
		Assert.That(() => CreateInstance(string.Empty, NomValide), Throws.ArgumentException);
		Assert.That(() => CreateInstance(" ", NomValide), Throws.ArgumentException);
	}

	[Test]
	public void Constructor_WhenGivenNullOrWhitespaceNom_ShouldThrowArgumentException()
	{
		Assert.That(() => CreateInstance(PrenomValide, null), Throws.ArgumentException);
		Assert.That(() => CreateInstance(PrenomValide, string.Empty), Throws.ArgumentException);
		Assert.That(() => CreateInstance(PrenomValide, " "), Throws.ArgumentException);
	}

	[Test]
	public void NomComplet_Always_ShouldBeEqualToPrenomAndNom()
	{
		Assert.That(Entite.NomComplet, Is.EqualTo($"{PrenomValide} {NomValide}"));
	}

	[Test]
	public void Equals_WhenGivenPersonneWithDifferentNomCompletAndId_ShouldReturnFalse()
	{
		// Arrange
		var autre = CreateInstance("Michel", "Berger");
		autre.SetId(Guid.NewGuid());

		// Assert
		Assert.That(Entite.Equals(autre), Is.False);
	}

	[Test]
	public void Equals_WhenGivenPersonneWithSameNomCompletAndId_ShouldReturnTrue()
	{
		// Arrange
		var autre = CreateInstance(PrenomValide, NomValide);

		// Assert
		Assert.That(Entite.Equals(autre), Is.True);
	}

	[Test]
	public void CompareTo_WhenGivenNull_ShouldReturn1()
	{
		// Act
		var result = Entite.CompareTo(null);

		// Assert
		Assert.That(result, Is.EqualTo(1));
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
	public void CompareTo_WhenGivenDifferentInstance_ShouldReturnComparisonResult()
	{
		// Arrange
		var autre = CreateInstance(PrenomValide, AutreNomValide);
		autre.SetId(Guid.NewGuid());

		// Act
		var result = Entite.CompareTo(autre);

		// Assert
		Assert.That(result, Is.EqualTo(string.Compare(Entite.NomComplet, autre.NomComplet, StringComparison.Ordinal)));
	}
}