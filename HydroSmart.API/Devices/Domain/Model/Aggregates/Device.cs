namespace HydroSmart.API.Devices.Domain.Model.Aggregates;

public class Device
{
    public int Id { get; }
    public string Name { get; private set; }
    public string Section { get; private set; }
    public string Status { get; private set; }
    public string LastActive { get; private set; }
    public int Alerts { get; private set; }
    public decimal Consumption { get; private set; }

    public Device()
    {
        Name = string.Empty;
        Section = string.Empty;
        Status = "inactive";
        LastActive = "0 h";
        Alerts = 0;
        Consumption = 0;
    }

    public Device(string name, string section, string status, string lastActive, int alerts, decimal consumption)
    {
        Name = name;
        Section = section;
        Status = status;
        LastActive = lastActive;
        Alerts = alerts;
        Consumption = consumption;
    }

    public Device UpdateInformation(
        string name,
        string section,
        string status,
        string lastActive,
        int alerts,
        decimal consumption)
    {
        Name = name;
        Section = section;
        Status = status;
        LastActive = lastActive;
        Alerts = alerts;
        Consumption = consumption;

        return this;
    }
}