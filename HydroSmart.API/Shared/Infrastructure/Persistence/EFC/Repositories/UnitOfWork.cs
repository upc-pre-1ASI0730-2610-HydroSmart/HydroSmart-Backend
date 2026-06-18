using HydroSmart.API.Shared.Domain.Repositories;
using HydroSmart.API.Shared.Infrastructure.Persistence.EFC.Configuration;

namespace HydroSmart.API.Shared.Infrastructure.Persistence.EFC.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task CompleteAsync()
    {
        await _context.SaveChangesAsync();
    }
}