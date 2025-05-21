using FluentValidation;
using Hospital_Management.ViewModels;

namespace Hospital_Management.Validations.Patients;

public class PatientCreateVMValidator : AbstractValidator<PatientCreateVM>
{
    public PatientCreateVMValidator()
    {
        RuleFor(x => x.AppUser)
            .NotEmpty().WithMessage("İstifadəçi ilə əlaqə boş ola bilməz.");
    }
}
