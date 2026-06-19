using HydroSmart.API.Analytics.Domain.Model.Aggregates;
using HydroSmart.API.Analytics.Domain.Model.Commands;

namespace HydroSmart.API.Analytics.Domain.Services;

public interface IWaterConsumptionRecordCommandService
{
    Task<WaterConsumptionRecord?> Handle(CreateWaterConsumptionRecordCommand command);
}
