namespace Hospital_Management.ViewModels;

public record RegisterVM
{
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public DateTime? DateOfBirth { get; set; }
    public IFormFile? Image { get; set; }
    public string Password { get; set; } = null!;
}
