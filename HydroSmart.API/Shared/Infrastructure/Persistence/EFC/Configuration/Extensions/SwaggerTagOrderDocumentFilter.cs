using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace HydroSmart.API.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;

public class SwaggerTagOrderDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        if (swaggerDoc.Tags is null || swaggerDoc.Tags.Count == 0)
        {
            return;
        }

        var order = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
        {
            ["Devices"] = 1,
            ["Profiles"] = 2,
            ["Analytics"] = 3,
            ["Notifications"] = 4,
            ["Settings"] = 5
        };

        swaggerDoc.Tags = swaggerDoc.Tags
            .OrderBy(tag => order.TryGetValue(tag.Name, out var value) ? value : int.MaxValue)
            .ThenBy(tag => tag.Name)
            .ToList();
    }
}