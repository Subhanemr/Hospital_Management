namespace Hospital_Management.ViewModels;

public record AppointmentGetVM : BaseEntityVM
{
    public DateTime? AppointmentDate { get; set; }
    public string? Status { get; set; }
    public string? Notes { get; set; }

    public string? DoctorId { get; set; }
    public DoctorIncludeVM? Doctor { get; set; }
    public string? PatientId { get; set; }
    public PatientIncludeVM? Patient { get; set; }
}
