using CineQuebec.Domain.Entities.Abstract;

namespace Tests.Domain.Entities.Abstract;

public abstract class PersonneTests<T> : EntiteTests<T> where T : Personne
{
	private const string PrenomValide = "Michel";
	private const string NomValide = "Sardou";
	protected override object?[] ArgsConstructeur => [PrenomValide, NomValide];

	[Test]
	public override void ToString_ShouldUniquelyDescribeTheEntity()
	{
		// Assert
		Assert.That(Entite.ToString(), Is.EqualTo($"{PrenomValide} {NomValide}"));
	}

	[Test]
	public void AfterInstantiation_PrenomShouldBeEqualToPrenomGivenInConstructor()
	{
		// Assert
		Assert.That(Entite.Prenom, Is.EqualTo(PrenomValide));
	}

	[Test]
	public void AfterInstantiation_NomShouldBeEqualToNomGivenInConstructor()
	{
		// Assert
		Assert.That(Entite.Nom, Is.EqualTo(NomValide));
	}

	[Test]
	public void OnInstantiation_ConstructorShouldThrowArgumentExceptionWhenPrenomIsNullOrWhitespace()
	{
		Assert.That(() => CreateInstance(null, NomValide), Throws.ArgumentException);
		Assert.That(() => CreateInstance(string.Empty, NomValide), Throws.ArgumentException);
		Assert.That(() => CreateInstance(" ", NomValide), Throws.ArgumentException);
	}

	[Test]
	public void OnInstantiation_ConstructorShouldThrowArgumentExceptionWhenNomIsNullOrWhitespace()
	{
		Assert.That(() => CreateInstance(PrenomValide, null), Throws.ArgumentException);
		Assert.That(() => CreateInstance(PrenomValide, string.Empty), Throws.ArgumentException);
		Assert.That(() => CreateInstance(PrenomValide, " "), Throws.ArgumentException);
	}

	[Test]
	public void AfterInstantiation_NomCompletShouldBeEqualToPrenomAndNomGivenInConstructor()
	{
		Assert.That(Entite.NomComplet, Is.EqualTo($"{PrenomValide} {NomValide}"));
	}
}