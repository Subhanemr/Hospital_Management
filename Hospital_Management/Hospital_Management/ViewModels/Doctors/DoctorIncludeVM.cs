namespace Hospital_Management.ViewModels;

public record DoctorIncludeVM : BaseEntityVM
{
    public string? Specialty { get; set; }
    public string? WorkingHours { get; set; }
    public string? RoomNumber { get; set; }

    public string? AppUserId { get; set; }
}
