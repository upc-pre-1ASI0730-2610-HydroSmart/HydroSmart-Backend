using HydroSmart.API.IAM.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace HydroSmart.API.IAM.Infrastructure.Persistence.EFC.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyIamConfiguration(this ModelBuilder builder)
    {
        builder.Entity<User>().HasKey(u => u.Id);

        builder.Entity<User>()
            .Property(u => u.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Entity<User>()
            .Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.Entity<User>()
            .Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(255);

        builder.Entity<User>()
            .Property(u => u.Role)
            .IsRequired()
            .HasMaxLength(50);

        builder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        builder.Entity<User>()
            .ToTable("users");
    }
}