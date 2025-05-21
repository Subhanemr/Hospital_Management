namespace Hospital_Management.Entities;

public class Appointment : BaseEntity
{
    public DateTime AppointmentDate { get; set; }
    public string Status { get; set; }
    public string? Notes { get; set; }

    public string DoctorId { get; set; }
    public Doctor Doctor { get; set; } = null!;
    public string PatientId { get; set; }
    public Patient Patient { get; set; } = null!;
}
