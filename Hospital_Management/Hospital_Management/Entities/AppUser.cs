using Microsoft.AspNetCore.Identity;

namespace Hospital_Management.Entities;

public class AppUser : IdentityUser
{
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public DateTime? DateOfBirth { get; set; }
    public string? Url { get; set; }
    public bool IsActivate { get; set; } = true;

    public string? DoctorId { get; set; }
    public Doctor? Doctor { get; set; }
    public string? PatientId { get; set; }
    public Patient? Patient { get; set; }
}
