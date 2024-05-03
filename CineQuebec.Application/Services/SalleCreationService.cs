using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services;
using CineQuebec.Application.Services.Abstract;
using CineQuebec.Domain.Entities.Projections;
using CineQuebec.Domain.Interfaces.Entities.Projections;

namespace CineQuebec.Application.Services;

public class SalleCreationService(IUnitOfWorkFactory unitOfWorkFactory) : ServiceAvecValidation, ISalleCreationService
{
    public async Task<Guid> CreerSalle(byte numero, ushort nbSieges)
    {
        using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();

        EffectuerValidations(unitOfWork, numero, nbSieges);
        ISalle salleAjoutee = await CreerNouvSalle(unitOfWork, numero, nbSieges);

        await unitOfWork.SauvegarderAsync();
        return salleAjoutee.Id;
    }

    private static void EffectuerValidations(IUnitOfWork unitOfWork, byte numero,
        ushort nbSieges)
    {
        LeverAggregateExceptionAuBesoin(
            ValiderNumero(numero),
            ValiderNbSieges(nbSieges),
            ValiderSalleEstUnique(unitOfWork, numero)
        );
    }

    private static async Task<ISalle> CreerNouvSalle(IUnitOfWork unitOfWork, byte numero,
        ushort nbSieges)
    {
        Salle salle = new(numero, nbSieges);

        return await unitOfWork.SalleRepository.AjouterAsync(salle);
    }

    private static IEnumerable<ArgumentException> ValiderNbSieges(ushort nbSieges)
    {
        if (nbSieges == 0)
        {
            yield return new ArgumentException("Le nombre de sièges de la salle doit être supérieur à 0.");
        }
    }

    private static IEnumerable<ArgumentException> ValiderNumero(byte numero)
    {
        if (numero == 0)
        {
            yield return new ArgumentException("Le numéro de la salle doit être supérieur à 0.");
        }
    }

    private static async IAsyncEnumerable<ArgumentException> ValiderSalleEstUnique(IUnitOfWork unitOfWork, byte numero)
    {
        if (await unitOfWork.SalleRepository.ExisteAsync(s => s.Numero == numero))
        {
            yield return new ArgumentException("Une salle avec ce numéro existe déjà.");
        }
    }
}