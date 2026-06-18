namespace HydroSmart.API.Shared.Domain.Repositories;

public interface IUnitOfWork
{
    /// <summary>
    ///     Commit changes to the database
    /// </summary>
    Task CompleteAsync();
}