namespace Hospital_Management.ViewModels;

public record DoctorUpdateVM
{
    public string Id { get; set; } = null!;
    public string Specialty { get; set; } = null!;
    public string WorkingHours { get; set; } = null!;
    public string RoomNumber { get; set; } = null!;

    public string AppUserId { get; set; } = null!;
    public AppUserIncludeVM AppUser { get; set; } = null!;
}
