namespace Hospital_Management.ViewModels;

public record DoctorCreateVM
{
    public string Specialty { get; set; } = null!;
    public string WorkingHours { get; set; } = null!;
    public string RoomNumber { get; set; } = null!;

    public RegisterVM AppUser { get; set; } = null!;
}
