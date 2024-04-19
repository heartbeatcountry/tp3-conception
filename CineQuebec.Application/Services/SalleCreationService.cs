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

        await EffectuerValidations(unitOfWork, numero, nbSieges);
        ISalle salleAjoutee = await CreerNouvSalle(unitOfWork, numero, nbSieges);

        await unitOfWork.SauvegarderAsync();
        return salleAjoutee.Id;
    }

    private static async Task EffectuerValidations(IUnitOfWork unitOfWork, byte numero,
        ushort nbSieges)
    {
        LeverAggregateExceptionAuBesoin(
            ValiderNumero(numero),
            ValiderNbSieges(nbSieges),
            await ValiderSalleEstUnique(unitOfWork, numero)
        );
    }

    private static async Task<ISalle> CreerNouvSalle(IUnitOfWork unitOfWork, byte numero,
        ushort nbSieges)
    {
        Salle salle = new(numero, nbSieges);

        return await unitOfWork.SalleRepository.AjouterAsync(salle);
    }

    private static ArgumentException? ValiderNbSieges(ushort nbSieges)
    {
        return nbSieges == 0 ? new ArgumentException("Le nombre de sièges de la salle doit être supérieur à 0.") : null;
    }

    private static ArgumentException? ValiderNumero(byte numero)
    {
        return numero == 0 ? new ArgumentException("Le numéro de la salle doit être supérieur à 0.") : null;
    }

    private static async Task<ArgumentException?> ValiderSalleEstUnique(IUnitOfWork unitOfWork, byte numero)
    {
        return await unitOfWork.SalleRepository.ExisteAsync(s => s.Numero == numero)
            ? new ArgumentException("Une salle avec ce numéro existe déjà.")
            : null;
    }
}