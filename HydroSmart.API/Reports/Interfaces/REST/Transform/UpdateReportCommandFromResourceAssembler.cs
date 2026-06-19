using HydroSmart.API.Reports.Domain.Model.Commands;
using HydroSmart.API.Reports.Interfaces.REST.Resources;

namespace HydroSmart.API.Reports.Interfaces.REST.Transform;

public class UpdateReportCommandFromResourceAssembler
{
    public static UpdateReportCommand ToCommandFromResource(UpdateReportResource resource, int reportId)
    {
        return new UpdateReportCommand(
            reportId,
            resource.Title,
            resource.Description,
            resource.Date,
            resource.Type
        );
    }
}

