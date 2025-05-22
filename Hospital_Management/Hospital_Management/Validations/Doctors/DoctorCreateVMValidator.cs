using FluentValidation;
using Hospital_Management.ViewModels;

namespace Hospital_Management.Validations.Doctors;

public class DoctorCreateVMValidator : AbstractValidator<DoctorCreateVM>
{
    public DoctorCreateVMValidator()
    {
        RuleFor(x => x.Specialty)
            .NotEmpty().WithMessage("İxtisas boş ola bilməz.")
            .MaximumLength(5000).WithMessage("İxtisas 100 simvoldan çox ola bilməz.");

        RuleFor(x => x.WorkingHours)
            .NotEmpty().WithMessage("İş saatları boş ola bilməz.");

        RuleFor(x => x.RoomNumber)
            .NotEmpty().WithMessage("Otaq nömrəsi boş ola bilməz.");

        RuleFor(x => x.AppUser)
            .NotEmpty().WithMessage("İstifadəçi ilə əlaqə boş ola bilməz.");
    }
}