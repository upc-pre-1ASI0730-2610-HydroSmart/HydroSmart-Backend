using HydroSmart.API.Reports.Domain.Model.Aggregates;
using HydroSmart.API.Reports.Domain.Model.Commands;
using HydroSmart.API.Reports.Domain.Repositories;
using HydroSmart.API.Reports.Domain.Services;

namespace HydroSmart.API.Reports.Application.Internal.CommandServices;

public class ReportCommandService : IReportCommandService
{
    private readonly IReportRepository _reportRepository;

    public ReportCommandService(IReportRepository reportRepository)
    {
        _reportRepository = reportRepository;
    }

    public async Task<Report?> Handle(CreateReportCommand command)
    {
        var report = new Report
        {
            Title = command.Title,
            Description = command.Description,
            Date = command.Date,
            Type = command.Type
        };

        try
        {
            var createdReport = await _reportRepository.AddAsync(report);
            return createdReport;
        }
        catch
        {
            return null;
        }
    }

    public async Task<Report?> Handle(UpdateReportCommand command)
    {
        var existingReport = await _reportRepository.GetByIdAsync(command.ReportId);
        
        if (existingReport is null)
            return null;

        existingReport.Title = command.Title;
        existingReport.Description = command.Description;
        existingReport.Date = command.Date;
        existingReport.Type = command.Type;

        try
        {
            var updatedReport = await _reportRepository.UpdateAsync(existingReport);
            return updatedReport;
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> HandleDelete(int reportId)
    {
        try
        {
            return await _reportRepository.DeleteAsync(reportId);
        }
        catch
        {
            return false;
        }
    }
}


