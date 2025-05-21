using FluentValidation;
using Hospital_Management.ViewModels;

namespace Hospital_Management.Validations.Accounts;

public class LoginVMValidator : AbstractValidator<LoginVM>
{
    public LoginVMValidator()
    {
        RuleFor(x => x.UserNameOrEmail)
            .NotEmpty().WithMessage("İstifadəçi adı və ya email tələb olunur.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Şifrə tələb olunur.")
            .MinimumLength(6).WithMessage("Şifrə ən azı 6 simvoldan ibarət olmalıdır.");
    }
}
