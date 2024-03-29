using CineQuebec.Domain.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Films;
using Moq;
using Tests.Domain.Entities.Abstract;

namespace Tests.Domain.Entities.Films;

public class FilmTests : EntiteTests<Film>
{
	private const string TitreValide = "Le Seigneur des Anneaux";
	private const string DescriptionValide = "Un film de Peter Jackson";
	private const ushort DureeValide = 178;
	private readonly IEnumerable<IActeur> _aucunActeurs = Array.Empty<IActeur>();
	private readonly IEnumerable<IRealisateur> _aucunRealisateurs = Array.Empty<IRealisateur>();
	private readonly ICategorieFilm _categorieFilm = Mock.Of<ICategorieFilm>(cf => cf.NomAffichage == "Action");
	private readonly DateTime _dateSortieInternationale = new(2001, 12, 19);

	protected override object?[] ArgsConstructeur =>
	[
		TitreValide, DescriptionValide, _categorieFilm, _dateSortieInternationale, _aucunActeurs,
		_aucunRealisateurs, DureeValide,
	];

	[Test]
	public void AfterInstantiation_ActeursShouldBeEmpty()
	{
		// Assert
		Assert.That(Entite.Acteurs, Is.Empty);
	}

	[Test]
	public void AfterInstantiation_CategorieShouldBeEqualToCategorieGivenInConstructor()
	{
		// Assert
		Assert.That(Entite.Categorie, Is.EqualTo(_categorieFilm));
	}

	[Test]
	public void AfterInstantiation_DateSortieInternationaleShouldBeEqualToDateSortieInternationaleGivenInConstructor()
	{
		// Assert
		Assert.That(Entite.DateSortieInternationale, Is.EqualTo(_dateSortieInternationale));
	}

	[Test]
	public void AfterInstantiation_DescriptionShouldBeEqualToDescriptionGivenInConstructor()
	{
		// Assert
		Assert.That(Entite.Description, Is.EqualTo(DescriptionValide));
	}

	[Test]
	public void AfterInstantiation_DureeShouldBeEqualToDureeGivenInConstructor()
	{
		// Assert
		Assert.That(Entite.Duree, Is.EqualTo(DureeValide));
	}

	[Test]
	public void AfterInstantiation_RealisateursShouldBeEmpty()
	{
		// Assert
		Assert.That(Entite.Realisateurs, Is.Empty);
	}

	[Test]
	public void AfterInstantiation_TitreShouldBeEqualToTitreGivenInConstructor()
	{
		// Assert
		Assert.That(Entite.Titre, Is.EqualTo(TitreValide));
	}

	[Test]
	public void OnInstantiation_ConstructorShouldThrowArgumentExceptionWhenDescriptionIsNullOrWhitespace()
	{
		Assert.That(() => CreateInstance(TitreValide, null, _categorieFilm, _dateSortieInternationale,
			_aucunActeurs, _aucunRealisateurs, DureeValide), Throws.ArgumentException);
		Assert.That(() => CreateInstance(TitreValide, string.Empty, _categorieFilm, _dateSortieInternationale,
			_aucunActeurs, _aucunRealisateurs, DureeValide), Throws.ArgumentException);
		Assert.That(() => CreateInstance(TitreValide, " ", _categorieFilm, _dateSortieInternationale,
			_aucunActeurs, _aucunRealisateurs, DureeValide), Throws.ArgumentException);
	}

	[Test]
	public void OnInstantiation_ConstructorShouldThrowArgumentExceptionWhenTitreIsNullOrWhitespace()
	{
		Assert.That(() => CreateInstance(null, DescriptionValide, _categorieFilm, _dateSortieInternationale,
			_aucunActeurs, _aucunRealisateurs, DureeValide), Throws.ArgumentException);
		Assert.That(() => CreateInstance(string.Empty, DescriptionValide, _categorieFilm, _dateSortieInternationale,
			_aucunActeurs, _aucunRealisateurs, DureeValide), Throws.ArgumentException);
		Assert.That(() => CreateInstance(" ", DescriptionValide, _categorieFilm, _dateSortieInternationale,
			_aucunActeurs, _aucunRealisateurs, DureeValide), Throws.ArgumentException);
	}

	[Test]
	public void OnInstantiation_ConstructorShouldThrowArgumentNullExceptionWhenCategorieIsNull()
	{
		Assert.That(() => CreateInstance(TitreValide, DescriptionValide, null, _dateSortieInternationale,
			_aucunActeurs, _aucunRealisateurs, DureeValide), Throws.ArgumentNullException);
	}

	[Test]
	public override void ToString_Always_ShouldUniquelyDescribeTheEntity()
	{
		// Assert
		Assert.That(Entite.ToString(), Is.EqualTo($"{Entite.Titre} ({Entite.DateSortieInternationale.Year})"));
	}
}