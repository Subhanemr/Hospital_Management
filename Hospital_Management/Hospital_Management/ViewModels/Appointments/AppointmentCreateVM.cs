namespace Hospital_Management.ViewModels;

public record AppointmentCreateVM
{
    public DateTime AppointmentDate { get; set; }
    public int Status { get; set; }
    public string? Notes { get; set; }

    public string DoctorId { get; set; } = null!;
    public string PatientId { get; set; } = null!;
}
