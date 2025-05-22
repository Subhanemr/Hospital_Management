using Hospital_Management.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital_Management.Configurations;

public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Specialty)
            .IsRequired()
            .HasMaxLength(5000);

        builder.Property(d => d.WorkingHours)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(d => d.RoomNumber)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(d => d.AppUserId)
            .IsRequired();

        builder.HasOne(d => d.AppUser)
            .WithOne(u => u.Doctor)
            .HasForeignKey<Doctor>(d => d.AppUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(d => d.Appointments)
            .WithOne(a => a.Doctor)
            .HasForeignKey(a => a.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
