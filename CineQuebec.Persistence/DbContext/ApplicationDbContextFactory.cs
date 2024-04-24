using CineQuebec.Persistence.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace CineQuebec.Persistence.DbContext;

public class ApplicationDbContextFactory(DbContextOptions<ApplicationDbContext> options) : IApplicationDbContextFactory
{
    public ApplicationDbContext Create()
    {
        return new ApplicationDbContext(options);
    }
}