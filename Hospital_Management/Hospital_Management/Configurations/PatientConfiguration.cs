using Hospital_Management.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital_Management.Configurations;

public class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.AppUserId)
               .IsRequired();

        builder.HasOne(p => p.AppUser)
               .WithOne(u => u.Patient)
               .HasForeignKey<Patient>(p => p.AppUserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.Appointments)
               .WithOne(a => a.Patient)
               .HasForeignKey(a => a.PatientId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.MedicalCards)
               .WithOne(mc => mc.Patient)
               .HasForeignKey(mc => mc.PatientId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}