using FluentValidation;
using Hospital_Management.ViewModels;

namespace Hospital_Management.Validations.AppUsers;

public class AppUserCreateVMValidator : AbstractValidator<AppUserCreateVM>
{
    public AppUserCreateVMValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().WithMessage("İstifadəçi adı tələb olunur.");
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email tələb olunur.").EmailAddress().WithMessage("Email formatı yanlışdır.");
        RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Telefon nömrəsi tələb olunur.");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Ad tələb olunur.");
        RuleFor(x => x.Surname).NotEmpty().WithMessage("Soyad tələb olunur.");
    }
}