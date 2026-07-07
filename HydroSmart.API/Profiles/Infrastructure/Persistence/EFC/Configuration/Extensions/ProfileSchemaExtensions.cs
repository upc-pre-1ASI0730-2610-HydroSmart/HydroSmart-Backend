using HydroSmart.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Microsoft.EntityFrameworkCore;

namespace HydroSmart.API.Profiles.Infrastructure.Persistence.EFC.Configuration.Extensions;

public static class ProfileSchemaExtensions
{
    public static async Task EnsureProfilePhotoSchemaAsync(this AppDbContext context)
    {
        const string sql = """
            ALTER TABLE `profiles`
            MODIFY COLUMN `photo_url` LONGTEXT NULL;
            """;

        await context.Database.ExecuteSqlRawAsync(sql);
    }
}
