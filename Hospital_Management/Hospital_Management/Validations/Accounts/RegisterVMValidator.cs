using FluentValidation;
using Hospital_Management.ViewModels;

namespace Hospital_Management.Validations.Accounts;

public class RegisterVMValidator : AbstractValidator<RegisterVM>
{
    public RegisterVMValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("İstifadəçi adı boş ola bilməz.")
            .MaximumLength(250).WithMessage("İstifadəçi adı 250 simvoldan çox ola bilməz.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email boş ola bilməz.")
            .EmailAddress().WithMessage("Email formatı yanlışdır.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Telefon nömrəsi boş ola bilməz.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Ad boş ola bilməz.")
            .MaximumLength(250).WithMessage("Ad 250 simvoldan çox ola bilməz.");

        RuleFor(x => x.Surname)
            .NotEmpty().WithMessage("Soyad boş ola bilməz.")
            .MaximumLength(250).WithMessage("Soyad 250 simvoldan çox ola bilməz.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Şifrə boş ola bilməz.")
            .MinimumLength(6).WithMessage("Şifrə ən azı 6 simvol olmalıdır.");
    }
}