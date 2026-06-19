using HydroSmart.API.Reports.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace HydroSmart.API.Reports.Infrastructure.Persistence.EFC.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyReportsConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Report>().HasKey(report => report.Id);

        builder.Entity<Report>()
            .Property(report => report.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Entity<Report>()
            .Property(report => report.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Entity<Report>()
            .Property(report => report.Description)
            .HasMaxLength(1000);

        builder.Entity<Report>()
            .Property(report => report.Date)
            .IsRequired();

        builder.Entity<Report>()
            .Property(report => report.Type)
            .IsRequired();

        builder.Entity<Report>()
            .HasIndex(report => report.Date);

        builder.Entity<Report>()
            .HasIndex(report => report.Type);


        builder.Entity<Report>()
            .ToTable("reports");
    }
}

