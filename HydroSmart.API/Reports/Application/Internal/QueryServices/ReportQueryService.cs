using HydroSmart.API.Reports.Domain.Model.Aggregates;
using HydroSmart.API.Reports.Domain.Model.Queries;
using HydroSmart.API.Reports.Domain.Repositories;
using HydroSmart.API.Reports.Domain.Services;

namespace HydroSmart.API.Reports.Application.Internal.QueryServices;

public class ReportQueryService : IReportQueryService
{
    private readonly IReportRepository _reportRepository;

    public ReportQueryService(IReportRepository reportRepository)
    {
        _reportRepository = reportRepository;
    }

    public async Task<Report?> Handle(GetReportByIdQuery query)
    {
        return await _reportRepository.GetByIdAsync(query.ReportId);
    }

    public async Task<IEnumerable<Report>> Handle(GetAllReportsQuery query)
    {
        return await _reportRepository.GetAllAsync();
    }
}

