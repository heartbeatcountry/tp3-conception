using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Domain.Entities.Films;
using CineQuebec.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CineQuebec.ConsoleApp;

internal class UOWTest
{
	private readonly IUnitOfWork _unitOfWork;

	public UOWTest(IUnitOfWorkFactory unitOfWorkFactory)
	{
		_unitOfWork = unitOfWorkFactory.Create();
		Test();
	}

	private async void Test()
	{
		var realisateur = new Realisateur("Test", "Test");
		await _unitOfWork.RealisateurRepository.AjouterAsync(realisateur);

		var film = new Film("Test", "Test", Guid.NewGuid(), DateOnly.Parse("2022-02-01").ToDateTime(TimeOnly.MinValue),
			Enumerable
				.Empty<Guid>
					(), new List<Guid> { realisateur.Id }, 22);
		await _unitOfWork.FilmRepository.AjouterAsync(film);

		await _unitOfWork.SauvegarderAsync();

		var films = await _unitOfWork.FilmRepository.ObtenirTousAsync();
	}
}

internal static class Program
{
	public static void Main()
	{
		var builder = Host.CreateApplicationBuilder();
		builder.Configuration.AddJsonFile("appsettings.local.json", true);

		builder.Services.AddPersistenceServices(builder.Configuration)
			.AddSingleton<UOWTest>();

		using var host = builder.Build();

		var service = host.Services.GetRequiredService<UOWTest>();

		host.Run();
	}
}