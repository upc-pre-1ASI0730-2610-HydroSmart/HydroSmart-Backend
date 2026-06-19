using HydroSmart.API.Analytics.Domain.Model.Aggregates;
using HydroSmart.API.Analytics.Domain.Model.Commands;
using HydroSmart.API.Analytics.Domain.Repositories;
using HydroSmart.API.Analytics.Domain.Services;
using HydroSmart.API.Shared.Domain.Repositories;

namespace HydroSmart.API.Analytics.Application.Internal.CommandServices;

public class WaterConsumptionRecordCommandService : IWaterConsumptionRecordCommandService
{
    private readonly IWaterConsumptionRecordRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public WaterConsumptionRecordCommandService(
        IWaterConsumptionRecordRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<WaterConsumptionRecord?> Handle(CreateWaterConsumptionRecordCommand command)
    {
        var record = new WaterConsumptionRecord(command.UserId, command.Liters, command.Category, command.RecordedAt);

        try
        {
            await _repository.AddAsync(record);
            await _unitOfWork.CompleteAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error creating water consumption record: {ex.Message}");
        }

        return record;
    }
}
