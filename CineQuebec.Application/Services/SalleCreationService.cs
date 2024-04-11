using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services;
using CineQuebec.Domain.Entities.Projections;
using CineQuebec.Domain.Interfaces.Entities.Projections;

namespace CineQuebec.Application.Services;

public class SalleCreationService(IUnitOfWorkFactory unitOfWorkFactory) : ISalleCreationService
{
    public async Task<Guid> CreerSalle(byte numero, ushort nbSieges)
    {
        using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();

        IEnumerable<Exception> exceptions = await EffectuerValidations(unitOfWork, numero, nbSieges);

        if (exceptions.ToArray() is { Length: > 0 } innerExceptions)
        {
            throw new AggregateException("Des erreurs se sont produites lors de la validation des données.",
                innerExceptions);
        }

        var salle = new Salle(numero, nbSieges);
        var salleAjoutee = await unitOfWork.SalleRepository.AjouterAsync(salle);

        await unitOfWork.SauvegarderAsync();
        return salleAjoutee.Id;
    }

    private static async Task<IEnumerable<Exception>> EffectuerValidations(IUnitOfWork unitOfWork, byte numero,
        ushort nbSieges)
    {
        List<Exception> exceptions = [];

        exceptions.AddRange(ValiderNumero(numero));
        exceptions.AddRange(ValiderNbSieges(nbSieges));
        exceptions.AddRange(await ValiderSalleEstUnique(unitOfWork, numero));

        return exceptions;
    }

    private static IEnumerable<Exception> ValiderNumero(byte numero)
    {
        List<Exception> exceptions = [];

        if (numero == 0)
        {
            exceptions.Add(new ArgumentException("Le numéro de la salle doit être supérieur à 0."));
        }

        return exceptions;
    }

    private static IEnumerable<Exception> ValiderNbSieges(ushort nbSieges)
    {
        List<Exception> exceptions = [];

        if (nbSieges == 0)
        {
            exceptions.Add(new ArgumentException("Le nombre de sièges de la salle doit être supérieur à 0."));
        }

        return exceptions;
    }

    private static async Task<IEnumerable<Exception>> ValiderSalleEstUnique(IUnitOfWork unitOfWork, byte numero)
    {
        List<Exception> exceptions = [];

        if (await unitOfWork.SalleRepository.ExisteAsync(s => s.Numero == numero))
        {
            exceptions.Add(new ArgumentException("Une salle avec ce numéro existe déjà."));
        }

        return exceptions;
    }
}