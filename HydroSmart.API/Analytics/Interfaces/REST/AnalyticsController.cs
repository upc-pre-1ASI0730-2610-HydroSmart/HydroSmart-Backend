using HydroSmart.API.Analytics.Interfaces.ACL;
using HydroSmart.API.Analytics.Interfaces.REST.Resources;
using Microsoft.AspNetCore.Mvc;

namespace HydroSmart.API.Analytics.Interfaces.REST;

[ApiController]
[Route("api/v1/analytics")]
public class AnalyticsController : ControllerBase
{
    private readonly IAnalyticsContextFacade _analyticsContextFacade;

    public AnalyticsController(IAnalyticsContextFacade analyticsContextFacade)
    {
        _analyticsContextFacade = analyticsContextFacade;
    }

    /// <summary>
    /// Get full dashboard analytics for a user
    /// </summary>
    [HttpGet("dashboard/{userId:int}")]
    public async Task<ActionResult<DashboardResource>> GetDashboard(int userId)
    {
        var dashboard = await _analyticsContextFacade.GetDashboard(userId);
        return Ok(dashboard);
    }

    /// <summary>
    /// Create a water consumption record
    /// </summary>
    [HttpPost("records")]
    public async Task<ActionResult<WaterConsumptionRecordResource>> CreateRecord([FromBody] CreateWaterConsumptionRecordRequest request)
    {
        var record = await _analyticsContextFacade.CreateRecord(request);
        return CreatedAtAction(nameof(GetDashboard), new { userId = record?.UserId }, record);
    }
}
