namespace Hospital_Management.ViewModels;

public record AppUserIncludeVM
{
    public string Id { get; set; } = null!;
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Url { get; set; }

    public string? DoctorId { get; set; }
    public string? PatientId { get; set; }
}
