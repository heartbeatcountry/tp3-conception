using CineQuebec.Persistence.DbContext;

namespace CineQuebec.Persistence.Interfaces;

public interface IApplicationDbContextFactory
{
    public ApplicationDbContext Create();
}