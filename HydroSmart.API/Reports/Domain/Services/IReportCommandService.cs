using HydroSmart.API.Reports.Domain.Model.Aggregates;
using HydroSmart.API.Reports.Domain.Model.Commands;

namespace HydroSmart.API.Reports.Domain.Services;

public interface IReportCommandService
{
    Task<Report?> Handle(CreateReportCommand command);
    Task<Report?> Handle(UpdateReportCommand command);
    Task<bool> HandleDelete(int reportId);
}

