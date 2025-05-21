namespace Hospital_Management.Entities;

public class Patient : BaseEntity
{
    public string AppUserId { get; set; } = null!;
    public AppUser AppUser { get; set; } = null!;
    public ICollection<Appointment>? Appointments { get; set; }
    public ICollection<MedicalCard>? MedicalCards { get; set; }
}
