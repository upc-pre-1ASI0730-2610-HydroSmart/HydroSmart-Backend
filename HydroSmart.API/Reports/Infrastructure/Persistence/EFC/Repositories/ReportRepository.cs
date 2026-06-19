using HydroSmart.API.Reports.Domain.Model.Aggregates;
using HydroSmart.API.Reports.Domain.Repositories;
using HydroSmart.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Microsoft.EntityFrameworkCore;

namespace HydroSmart.API.Reports.Infrastructure.Persistence.EFC.Repositories;

public class ReportRepository : IReportRepository
{
    private readonly AppDbContext _context;

    public ReportRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Report?> GetByIdAsync(int reportId)
    {
        return await _context.Reports.FirstOrDefaultAsync(r => r.Id == reportId);
    }

    public async Task<IEnumerable<Report>> GetAllAsync()
    {
        return await _context.Reports
            .OrderByDescending(r => r.Date)
            .ToListAsync();
    }

    public async Task<Report> AddAsync(Report report)
    {
        _context.Reports.Add(report);
        await _context.SaveChangesAsync();
        return report;
    }

    public async Task<Report> UpdateAsync(Report report)
    {
        _context.Reports.Update(report);
        await _context.SaveChangesAsync();
        return report;
    }

    public async Task<bool> DeleteAsync(int reportId)
    {
        var report = await _context.Reports.FirstOrDefaultAsync(r => r.Id == reportId);
        if (report is null)
            return false;

        _context.Reports.Remove(report);
        await _context.SaveChangesAsync();
        return true;
    }
}

