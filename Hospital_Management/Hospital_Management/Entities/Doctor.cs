namespace Hospital_Management.Entities;

public class Doctor : BaseEntity
{
    public string Specialty { get; set; } = null!;
    public string WorkingHours { get; set; } = null!;
    public string RoomNumber { get; set; } = null!;

    public string AppUserId { get; set; } = null!;
    public AppUser AppUser { get; set; } = null!;

    public ICollection<Appointment>? Appointments { get; set; }
}
