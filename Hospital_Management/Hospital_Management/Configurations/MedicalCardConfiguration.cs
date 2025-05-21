using Hospital_Management.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital_Management.Configurations
{
    public class MedicalCardConfiguration : IEntityTypeConfiguration<MedicalCard>
    {
        public void Configure(EntityTypeBuilder<MedicalCard> builder)
        {
            builder.HasKey(mc => mc.Id);

            builder.Property(mc => mc.DiseaseHistory)
                   .IsRequired()
                   .HasMaxLength(10000);

            builder.Property(mc => mc.LabResults)
                   .IsRequired()
                   .HasMaxLength(10000);

            builder.Property(mc => mc.PatientId)
                   .IsRequired();

            builder.HasOne(mc => mc.Patient)
                   .WithMany(p => p.MedicalCards)
                   .HasForeignKey(mc => mc.PatientId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
