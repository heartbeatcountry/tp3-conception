namespace CineQuebec.Application.Interfaces.DbContext;

public interface IUnitOfWorkFactory
{
    public IUnitOfWork Create();
}