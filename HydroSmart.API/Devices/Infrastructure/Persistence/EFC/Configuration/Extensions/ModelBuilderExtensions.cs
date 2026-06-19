using HydroSmart.API.Devices.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace HydroSmart.API.Devices.Infrastructure.Persistence.EFC.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyDevicesConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Device>().HasKey(device => device.Id);

        builder.Entity<Device>()
            .Property(device => device.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Entity<Device>()
            .Property(device => device.Name)
            .IsRequired()
            .HasMaxLength(120);

        builder.Entity<Device>()
            .Property(device => device.Section)
            .IsRequired()
            .HasMaxLength(80);

        builder.Entity<Device>()
            .Property(device => device.Status)
            .IsRequired()
            .HasMaxLength(30);

        builder.Entity<Device>()
            .Property(device => device.LastActive)
            .IsRequired()
            .HasMaxLength(30);

        builder.Entity<Device>()
            .Property(device => device.Alerts)
            .IsRequired();

        builder.Entity<Device>()
            .Property(device => device.Consumption)
            .IsRequired()
            .HasColumnType("decimal(10,2)");

        builder.Entity<Device>()
            .HasIndex(device => device.Section);

        builder.Entity<Device>()
            .HasIndex(device => device.Status);

        builder.Entity<Device>()
            .ToTable("devices");

        builder.Entity<Device>().HasData(
            new
            {
                Id = 1,
                Name = "Bathroom Sink 1",
                Section = "Bathroom",
                Status = "active",
                LastActive = "2 h",
                Alerts = 0,
                Consumption = 800m
            },
            new
            {
                Id = 2,
                Name = "Bathroom Sink 2",
                Section = "Bathroom",
                Status = "active",
                LastActive = "6 h",
                Alerts = 2,
                Consumption = 352m
            },
            new
            {
                Id = 3,
                Name = "Kitchen Sink 1",
                Section = "Kitchen",
                Status = "inactive",
                LastActive = "10 h",
                Alerts = 1,
                Consumption = 527m
            }
        );
    }
}