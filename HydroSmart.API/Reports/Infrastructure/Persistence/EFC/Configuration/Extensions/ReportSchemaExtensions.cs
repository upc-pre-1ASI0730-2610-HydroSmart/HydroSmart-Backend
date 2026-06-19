using HydroSmart.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Microsoft.EntityFrameworkCore;

namespace HydroSmart.API.Reports.Infrastructure.Persistence.EFC.Configuration.Extensions;

public static class ReportSchemaExtensions
{
    public static async Task EnsureReportsSchemaAsync(this AppDbContext context)
    {
        const string sql = """
            CREATE TABLE IF NOT EXISTS `reports` (
                `id` int NOT NULL AUTO_INCREMENT,
                `title` varchar(200) NOT NULL,
                `description` varchar(1000) NULL,
                `date` datetime(6) NOT NULL,
                `type` int NOT NULL,
                PRIMARY KEY (`id`),
                INDEX `ix_reports_date` (`date`),
                INDEX `ix_reports_type` (`type`)
            ) CHARACTER SET=utf8mb4;
            """;

        await context.Database.ExecuteSqlRawAsync(sql);
    }
}
