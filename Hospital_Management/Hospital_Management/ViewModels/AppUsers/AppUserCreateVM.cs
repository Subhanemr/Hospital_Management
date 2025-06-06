﻿namespace Hospital_Management.ViewModels;

public record AppUserCreateVM
{
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public IFormFile? Image { get; set; }
}
