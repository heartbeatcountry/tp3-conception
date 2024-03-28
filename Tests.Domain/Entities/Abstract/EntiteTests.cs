using System.Reflection;
using CineQuebec.Domain.Entities.Abstract;

// ReSharper disable EqualExpressionComparison

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
	public void Constructor_Always_ShouldCreateEntityOfTypeT()
	{
		Assert.That(Entite, Is.InstanceOf<T>());
	}

	[Test]
	public void Constructor_Always_ShouldSetIdToEmptyGuid()
	{
		// Assert
		Assert.That(Entite.Id, Is.EqualTo(Guid.Empty));
	}

	[Test]
	public void SetId_WhenGivenEmptyGuid_ShouldThrowArgumentNullException()
	{
		// Assert
		Assert.That(() => Entite.SetId(Guid.Empty), Throws.ArgumentNullException);
	}

	[Test]
	public void SetId_WhenGivenNonEmptyGuid_ShouldSetId()
	{
		// Arrange
		var id = Guid.NewGuid();

		// Act
		Entite.SetId(id);

		// Assert
		Assert.That(Entite.Id, Is.EqualTo(id));
	}

	[Test]
	public void Equals_WhenComparingToNull_ShouldReturnFalse()
	{
		// Assert
		Assert.That(Entite, Is.Not.EqualTo(null));
	}

	[Test]
	public void Equals_WhenComparingToSameInstance_ShouldReturnTrue()
	{
		// Assert
		Assert.That(Entite, Is.EqualTo(Entite));
	}

	[Test]
	public void Equals_WhenComparingToDifferentInstanceWithDifferentId_ShouldReturnFalse()
	{
		// Arrange
		var autre = CreateInstance();
		Entite.SetId(Guid.NewGuid());
		autre.SetId(Guid.NewGuid());

		// Assert
		Assert.That(Entite, Is.Not.EqualTo(autre));
	}

	[Test]
	public void Equals_WhenComparingToEntityOfDifferentType_ShouldReturnFalse()
	{
		// Arrange
		var autre = new object();

		// Assert
		Assert.That(Entite, Is.Not.EqualTo(autre));
	}

	[Test]
	public void Equals_WhenComparingToDifferentInstanceWithSameId_ShouldReturnTrue()
	{
		// Arrange
		var autre = CreateInstance();
		Entite.SetId(Guid.NewGuid());
		autre.SetId(Entite.Id);

		// Assert
		Assert.That(Entite, Is.EqualTo(autre));
	}

	[Test]
	public void OperatorEqual_WhenComparingToSameInstance_ShouldReturnTrue()
	{
		// Assert
		Assert.That(Entite == Entite, Is.True);
	}

	[Test]
	public void OperatorEqual_WhenComparingToDifferentInstanceWithSameId_ShouldReturnTrue()
	{
		// Arrange
		var autre = CreateInstance();
		Entite.SetId(Guid.NewGuid());
		autre.SetId(Entite.Id);

		// Assert
		Assert.That(Entite == autre, Is.True);
	}

	[Test]
	public void OperatorEqual_WhenComparingToDifferentInstanceWithDifferentId_ShouldReturnFalse()
	{
		// Arrange
		var autre = CreateInstance();
		Entite.SetId(Guid.NewGuid());
		autre.SetId(Guid.NewGuid());

		// Assert
		Assert.That(Entite == autre, Is.False);
	}

	[Test]
	public void OperatorNotEqual_WhenComparingDifferentInstanceWithSameId_ShouldReturnFalse()
	{
		// Arrange
		var autre = CreateInstance();
		Entite.SetId(Guid.NewGuid());
		autre.SetId(Entite.Id);

		// Assert
		Assert.That(Entite != autre, Is.False);
	}

	[Test]
	public void OperatorNotEqual_WhenComparingDifferentInstanceWithDifferentId_ShouldReturnTrue()
	{
		// Arrange
		var autre = CreateInstance();
		Entite.SetId(Guid.NewGuid());
		autre.SetId(Guid.NewGuid());

		// Assert
		Assert.That(Entite != autre, Is.True);
	}

	[Test]
	public void OperatorNotEqual_WhenComparingToSameInstance_ShouldReturnFalse()
	{
		// Assert
		Assert.That(Entite != Entite, Is.False);
	}

	[Test]
	public virtual void ToString_Always_ShouldUniquelyDescribeTheEntity()
	{
		// Arrange
		var type = Entite.GetType().Name;
		var id = Entite.Id;

		// Assert
		Assert.That(Entite.ToString(), Is.EqualTo($"{type} {id}"));
	}

	[Test]
	public virtual void GetHashCode_Always_ShouldBeUniqueToTheEntity()
	{
		// Arrange
		var id = Guid.NewGuid();
		Entite.SetId(id);

		// Assert
		Assert.That(Entite.GetHashCode(), Is.EqualTo(id.GetHashCode()));
	}

	[Test]
	public void Instance_WhenAddingToHashSet_ShouldNotAddOtherInstanceWithSameId()
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