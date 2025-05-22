namespace Hospital_Management.ViewModels;

public record PatientIncludeVM : BaseEntityVM
{
    public string? AppUserId { get; set; }
    public AppUserIncludeVM? AppUser { get; set; }

    public ICollection<MedicalCardIncludeVM>? MedicalCards { get; set; }
}
