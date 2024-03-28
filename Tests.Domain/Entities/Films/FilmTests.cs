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
	private readonly IActeur _acteur1 = Mock.Of<IActeur>();
	private readonly IActeur _acteur2 = Mock.Of<IActeur>();
	private readonly ICategorieFilm _categorieFilm = Mock.Of<ICategorieFilm>(cf => cf.NomAffichage == "Action");
	private readonly DateTime _dateSortieInternationale = new(2001, 12, 19);
	private readonly IRealisateur _realisateur1 = Mock.Of<IRealisateur>();
	private readonly IRealisateur _realisateur2 = Mock.Of<IRealisateur>();

	protected override object?[] ArgsConstructeur =>
	[
		TitreValide, DescriptionValide, _categorieFilm, _dateSortieInternationale, Array.Empty<IActeur>(),
		Array.Empty<IRealisateur>(), DureeValide,
	];

	[Test]
	public override void ToString_ShouldUniquelyDescribeTheEntity()
	{
		// Assert
		Assert.That(Entite.ToString(), Is.EqualTo($"{Entite.Titre} ({Entite.DateSortieInternationale.Year})"));
	}
}