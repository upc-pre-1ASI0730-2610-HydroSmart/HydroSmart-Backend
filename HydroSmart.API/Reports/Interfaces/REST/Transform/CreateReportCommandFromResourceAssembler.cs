using HydroSmart.API.Reports.Domain.Model.Commands;
using HydroSmart.API.Reports.Interfaces.REST.Resources;

namespace HydroSmart.API.Reports.Interfaces.REST.Transform;

public class CreateReportCommandFromResourceAssembler
{
    public static CreateReportCommand ToCommandFromResource(CreateReportResource resource)
    {
        return new CreateReportCommand(
            resource.Title,
            resource.Description,
            resource.Date,
            resource.Type
        );
    }
}

