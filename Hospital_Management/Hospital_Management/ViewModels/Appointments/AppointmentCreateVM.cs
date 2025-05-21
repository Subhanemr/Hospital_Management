namespace Hospital_Management.ViewModels;

public record AppointmentCreateVM
{
    public DateTime AppointmentDate { get; set; }
    public string Status { get; set; } = null!;
    public string? Notes { get; set; }

    public string DoctorId { get; set; } = null!;
    public string PatientId { get; set; } = null!;
}
