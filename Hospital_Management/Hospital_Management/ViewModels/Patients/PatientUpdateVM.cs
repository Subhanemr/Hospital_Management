namespace Hospital_Management.ViewModels;

public record PatientUpdateVM
{
    public string Id { get; set; } = null!;
    public string AppUserId { get; set; } = null!;
    public AppUserIncludeVM AppUser { get; set; } = null!;
}
