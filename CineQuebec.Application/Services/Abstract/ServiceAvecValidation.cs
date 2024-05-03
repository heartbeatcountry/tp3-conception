namespace CineQuebec.Application.Services.Abstract;

public abstract class ServiceAvecValidation
{
    protected static void LeverAggregateExceptionAuBesoin(params dynamic?[] lstExceptions)
    {
        if (ObtenirExceptions(lstExceptions) is { Count: > 0 } exceptions)
        {
            throw new AggregateException("Des erreurs se sont produites lors de la validation des données.",
                exceptions);
        }
    }

    private static List<Exception> ObtenirExceptions(IEnumerable<dynamic?> lstExceptions)
    {
        List<Exception> exceptions = [];

        foreach (dynamic? dynException in lstExceptions)
        {
            switch (dynException)
            {
                case IAsyncEnumerable<Exception> iasyncEnum:
                    exceptions.AddRange(iasyncEnum.ToBlockingEnumerable());
                    break;
                case IEnumerable<Exception> ienum:
                    exceptions.AddRange(ienum);
                    break;
                case Exception exception:
                    exceptions.Add(exception);
                    break;
            }
        }

        return exceptions;
    }
}