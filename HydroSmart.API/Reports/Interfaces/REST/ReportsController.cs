using System.Net.Mime;
using HydroSmart.API.Reports.Domain.Model.Queries;
using HydroSmart.API.Reports.Domain.Services;
using HydroSmart.API.Reports.Interfaces.REST.Resources;
using HydroSmart.API.Reports.Interfaces.REST.Transform;
using HydroSmart.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace HydroSmart.API.Reports.Interfaces.REST;

[ApiController]
[Route("reports")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Reports Endpoints.")]
public class ReportsController(
    IReportCommandService reportCommandService,
    IReportQueryService reportQueryService) : ControllerBase
{
    /// <summary>
    /// Get all reports
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    [SwaggerOperation("Get All Reports", "Get all reports.", OperationId = "GetAllReports")]
    [SwaggerResponse(200, "Reports found and returned.", typeof(IEnumerable<ReportResource>))]
    public async Task<IActionResult> GetAllReports()
    {
        var reports = await reportQueryService.Handle(new GetAllReportsQuery());
        var resources = reports.Select(ReportResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    /// <summary>
    /// Get a specific report by ID
    /// </summary>
    [HttpGet("{reportId:int}")]
    [AllowAnonymous]
    [SwaggerOperation("Get Report by Id", "Get a specific report by its unique identifier.", OperationId = "GetReportById")]
    [SwaggerResponse(200, "Report found and returned.", typeof(ReportResource))]
    [SwaggerResponse(404, "Report not found.")]
    public async Task<IActionResult> GetReportById(int reportId)
    {
        var report = await reportQueryService.Handle(new GetReportByIdQuery(reportId));

        if (report is null)
            return NotFound(new { message = "Report not found" });

        var resource = ReportResourceFromEntityAssembler.ToResourceFromEntity(report);
        return Ok(resource);
    }


    /// <summary>
    /// Create a new report
    /// </summary>
    [HttpPost]
    [AllowAnonymous]
    [SwaggerOperation("Create Report", "Create a new report.", OperationId = "CreateReport")]
    [SwaggerResponse(201, "Report created successfully.", typeof(ReportResource))]
    [SwaggerResponse(400, "Report could not be created.")]
    public async Task<IActionResult> CreateReport([FromBody] CreateReportResource resource)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var command = CreateReportCommandFromResourceAssembler.ToCommandFromResource(resource);
        var report = await reportCommandService.Handle(command);

        if (report is null)
            return BadRequest(new { message = "Failed to create report" });

        var reportResource = ReportResourceFromEntityAssembler.ToResourceFromEntity(report);
        return CreatedAtAction(
            nameof(GetReportById),
            new { reportId = report.Id },
            reportResource
        );
    }

    /// <summary>
    /// Update an existing report
    /// </summary>
    [HttpPut("{reportId:int}")]
    [AllowAnonymous]
    [SwaggerOperation("Update Report", "Update an existing report by its unique identifier.", OperationId = "UpdateReport")]
    [SwaggerResponse(200, "Report updated successfully.", typeof(ReportResource))]
    [SwaggerResponse(404, "Report not found.")]
    public async Task<IActionResult> UpdateReport(int reportId, [FromBody] UpdateReportResource resource)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var command = UpdateReportCommandFromResourceAssembler.ToCommandFromResource(resource, reportId);
        var report = await reportCommandService.Handle(command);

        if (report is null)
            return NotFound(new { message = "Report not found" });

        var reportResource = ReportResourceFromEntityAssembler.ToResourceFromEntity(report);
        return Ok(reportResource);
    }

    /// <summary>
    /// Delete a report
    /// </summary>
    [HttpDelete("{reportId:int}")]
    [AllowAnonymous]
    [SwaggerOperation("Delete Report", "Delete an existing report by its unique identifier.", OperationId = "DeleteReport")]
    [SwaggerResponse(204, "Report deleted successfully.")]
    [SwaggerResponse(404, "Report not found.")]
    public async Task<IActionResult> DeleteReport(int reportId)
    {
        var deleted = await reportCommandService.HandleDelete(reportId);

        if (!deleted)
            return NotFound(new { message = "Report not found" });

        return NoContent();
    }
}

