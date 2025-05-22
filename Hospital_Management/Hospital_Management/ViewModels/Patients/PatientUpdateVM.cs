namespace Hospital_Management.ViewModels;

public record PatientUpdateVM
{
    public string AppUserId { get; set; } = null!;
    public AppUserIncludeVM AppUser { get; set; } = null!;
}
