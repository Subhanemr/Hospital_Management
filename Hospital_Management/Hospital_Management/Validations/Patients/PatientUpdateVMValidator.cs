using FluentValidation;
using Hospital_Management.ViewModels;

namespace Hospital_Management.Validations.Patients;

public class PatientUpdateVMValidator : AbstractValidator<PatientCreateVM>
{
    public PatientUpdateVMValidator()
    {
        RuleFor(x => x.AppUser)
            .NotEmpty().WithMessage("İstifadəçi ilə əlaqə boş ola bilməz.");
    }
}
