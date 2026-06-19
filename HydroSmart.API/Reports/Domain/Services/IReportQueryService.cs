using HydroSmart.API.Reports.Domain.Model.Aggregates;
using HydroSmart.API.Reports.Domain.Model.Queries;

namespace HydroSmart.API.Reports.Domain.Services;

public interface IReportQueryService
{
    Task<Report?> Handle(GetReportByIdQuery query);
    Task<IEnumerable<Report>> Handle(GetAllReportsQuery query);
}

