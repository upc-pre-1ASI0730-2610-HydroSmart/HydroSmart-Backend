using HydroSmart.API.Profiles.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace HydroSmart.API.Profiles.Infrastructure.Persistence.EFC.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyProfilesConfiguration(this ModelBuilder builder)
    {
        // Profiles Context
        
        builder.Entity<Profile>().HasKey(p => p.Id);
        builder.Entity<Profile>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Profile>().Property(p => p.UserId).IsRequired();
        builder.Entity<Profile>().Property(p => p.PhotoUrl).HasMaxLength(500);
        builder.Entity<Profile>().Property(p => p.FirstName).IsRequired().HasMaxLength(200);
        builder.Entity<Profile>().Property(p => p.LastName).IsRequired().HasMaxLength(100);
        builder.Entity<Profile>().Property(p => p.Address).HasMaxLength(500);
        builder.Entity<Profile>().Property(p => p.Email).IsRequired().HasMaxLength(100);
        builder.Entity<Profile>().Property(p => p.PhoneNumber).HasMaxLength(20);
    }
}
