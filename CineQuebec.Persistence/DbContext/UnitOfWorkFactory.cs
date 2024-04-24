using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Persistence.Interfaces;

namespace CineQuebec.Persistence.DbContext;

public class UnitOfWorkFactory(IApplicationDbContextFactory applicationDbContextFactory) : IUnitOfWorkFactory
{
    public IUnitOfWork Create()
    {
        return new UnitOfWork(applicationDbContextFactory);
    }
}