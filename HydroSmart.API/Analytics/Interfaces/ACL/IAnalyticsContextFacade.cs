using HydroSmart.API.Analytics.Interfaces.REST.Resources;

namespace HydroSmart.API.Analytics.Interfaces.ACL;

public interface IAnalyticsContextFacade
{
    Task<DashboardResource> GetDashboard(int userId);
    Task<WaterConsumptionRecordResource?> CreateRecord(CreateWaterConsumptionRecordRequest request);
}
