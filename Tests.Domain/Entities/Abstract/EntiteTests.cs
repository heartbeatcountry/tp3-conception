using System.Reflection;
using CineQuebec.Domain.Entities.Abstract;

namespace Tests.Domain.Entities.Abstract;

public abstract class EntiteTests<T> where T : Entite
{
	protected virtual T Entite { get; set; } = null!;
	protected virtual object?[] ArgsConstructeur => [];

	protected T CreateInstance(params object?[] args)
	{
		try
		{
			return (T)Activator.CreateInstance(typeof(T), args)!;
		}
		catch (TargetInvocationException e)
		{
			throw e.InnerException!;
		}
	}

	protected T CreateInstance()
	{
		return CreateInstance(ArgsConstructeur);
	}

	[SetUp]
	public virtual void Setup()
	{
		Entite = CreateInstance();
	}

	[Test]
	public void AfterInstantiation_EntityShouldBeOfTypeT()
	{
		Assert.That(Entite, Is.InstanceOf<T>());
	}

	[Test]
	public void OnInstantiation_IdShouldBeEmpty()
	{
		// Assert
		Assert.That(Entite.Id, Is.EqualTo(Guid.Empty));
	}

	[Test]
	public void SetId_ShouldThrowArgumentNullExceptionWhenIdIsEmpty()
	{
		// Assert
		Assert.That(() => Entite.SetId(Guid.Empty), Throws.ArgumentNullException);
	}

	[Test]
	public void SetId_ShouldSetIdWhenGivenIdIsNotEmpty()
	{
		// Arrange
		var id = Guid.NewGuid();

		// Act
		Entite.SetId(id);

		// Assert
		Assert.That(Entite.Id, Is.EqualTo(id));
	}

	[Test]
	public void Equals_ShouldReturnFalseWhenGivenEntiteIsNull()
	{
		// Assert
		Assert.That(Entite, Is.Not.EqualTo(null));
	}

	[Test]
	public void Equals_ShouldReturnTrueWhenGivenEntiteIsSameInstance()
	{
		// Assert
		Assert.That(Entite, Is.EqualTo(Entite));
	}

	[Test]
	public void Equals_ShouldReturnFalseWhenGivenEntiteIsDifferentInstanceWithDifferentId()
	{
		// Arrange
		var autre = CreateInstance();
		Entite.SetId(Guid.NewGuid());
		autre.SetId(Guid.NewGuid());

		// Assert
		Assert.That(Entite, Is.Not.EqualTo(autre));
	}

	[Test]
	public void Equals_ShouldReturnFalseWhenGivenEntiteIsDifferentType()
	{
		// Arrange
		var autre = new object();

		// Assert
		Assert.That(Entite, Is.Not.EqualTo(autre));
	}

	[Test]
	public void Equals_ShouldReturnTrueWhenGivenEntiteHasSameId()
	{
		// Arrange
		var autre = CreateInstance();
		Entite.SetId(Guid.NewGuid());
		autre.SetId(Entite.Id);

		// Assert
		Assert.That(Entite, Is.EqualTo(autre));
	}

	[Test]
	public void Equals_ShouldReturnFalseWhenGivenEntiteHasDifferentId()
	{
		// Arrange
		var autre = CreateInstance();
		Entite.SetId(Guid.NewGuid());
		autre.SetId(Guid.NewGuid());

		// Assert
		Assert.That(Entite, Is.Not.EqualTo(autre));
	}

	[Test]
	public void OperatorEqual_ShouldReturnTrueWhenGivenEntiteHasSameId()
	{
		// Arrange
		var autre = CreateInstance();
		Entite.SetId(Guid.NewGuid());
		autre.SetId(Entite.Id);

		// Assert
		Assert.That(Entite == autre, Is.True);
	}

	[Test]
	public void OperatorEqual_ShouldReturnFalseWhenGivenEntiteHasDifferentId()
	{
		// Arrange
		var autre = CreateInstance();
		Entite.SetId(Guid.NewGuid());
		autre.SetId(Guid.NewGuid());

		// Assert
		Assert.That(Entite == autre, Is.False);
	}

	[Test]
	public void OperatorNotEqual_ShouldReturnFalseWhenGivenEntiteHasSameId()
	{
		// Arrange
		var autre = CreateInstance();
		Entite.SetId(Guid.NewGuid());
		autre.SetId(Entite.Id);

		// Assert
		Assert.That(Entite != autre, Is.False);
	}

	[Test]
	public void OperatorNotEqual_ShouldReturnTrueWhenGivenEntiteHasDifferentId()
	{
		// Arrange
		var autre = CreateInstance();
		Entite.SetId(Guid.NewGuid());
		autre.SetId(Guid.NewGuid());

		// Assert
		Assert.That(Entite != autre, Is.True);
	}

	[Test]
	public virtual void ToString_ShouldUniquelyDescribeTheEntity()
	{
		// Arrange
		var type = Entite.GetType().Name;
		var id = Entite.Id;

		// Assert
		Assert.That(Entite.ToString(), Is.EqualTo($"{type} {id}"));
	}

	[Test]
	public virtual void GetHashCode_ShouldBeUniqueToTheEntity()
	{
		// Arrange
		var id = Guid.NewGuid();
		Entite.SetId(id);

		// Assert
		Assert.That(Entite.GetHashCode(), Is.EqualTo(id.GetHashCode()));
	}

	[Test]
	public void AddingToHashSet_ShouldNotAddDuplicateWithSameId()
	{
		// Arrange
		var hashSet = new HashSet<T>();
		Entite.SetId(Guid.NewGuid());
		var autre = CreateInstance();
		autre.SetId(Entite.Id);
		hashSet.Add(Entite);

		// Act
		hashSet.Add(autre);

		// Assert
		Assert.That(hashSet, Has.Count.EqualTo(1));
	}
}