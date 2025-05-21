using Hospital_Management.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital_Management.Configurations;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Surname)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.DateOfBirth)
            .IsRequired(false);

        builder.Property(u => u.Url)
            .HasMaxLength(200);

        builder.HasOne(u => u.Doctor)
            .WithOne(d => d.AppUser)
            .HasForeignKey<Doctor>(d => d.AppUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(u => u.Patient)
            .WithOne(p => p.AppUser)
            .HasForeignKey<Patient>(p => p.AppUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}