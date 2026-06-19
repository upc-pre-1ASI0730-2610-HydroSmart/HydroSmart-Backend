using HydroSmart.API.Reports.Domain.Model.Aggregates;
using HydroSmart.API.Reports.Interfaces.REST.Resources;

namespace HydroSmart.API.Reports.Interfaces.REST.Transform;

public class ReportResourceFromEntityAssembler
{
    public static ReportResource ToResourceFromEntity(Report entity)
    {
        return new ReportResource
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            Date = entity.Date,
            Type = entity.Type
        };
    }
}

