namespace Hospital_Management.ViewModels;

public record PatientGetVM : BaseEntityVM
{
    public string? AppUserId { get; set; }
    public AppUserIncludeVM? AppUser { get; set; }
    public ICollection<AppointmentIncludeVM>? Appointments { get; set; }
    public ICollection<MedicalCardIncludeVM>? MedicalCards { get; set; }
}
