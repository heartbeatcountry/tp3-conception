using CineQuebec.Domain.Entities.Abstract;

namespace Tests.Domain.Entities.Abstract;

public abstract class PersonneTests<T> : EntiteTests<T> where T : Personne
{
	private const string PrenomValide = "Michel";
	private const string NomValide = "Sardou";
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
}