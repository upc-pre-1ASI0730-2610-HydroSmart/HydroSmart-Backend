using HydroSmart.API.Reports.Domain.Model.Aggregates;

namespace HydroSmart.API.Reports.Domain.Repositories;

public interface IReportRepository
{
    Task<Report?> GetByIdAsync(int reportId);
    Task<IEnumerable<Report>> GetAllAsync();
    Task<Report> AddAsync(Report report);
    Task<Report> UpdateAsync(Report report);
    Task<bool> DeleteAsync(int reportId);
}

