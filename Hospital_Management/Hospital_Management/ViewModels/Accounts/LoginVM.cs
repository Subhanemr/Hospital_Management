namespace Hospital_Management.ViewModels;

public class LoginVM
{
    public string UserNameOrEmail { get; init; } = null!;
    public string Password { get; set; } = null!;
    public bool IsRemembered { get; init; } 
}
