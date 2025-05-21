using FluentValidation;
using Hospital_Management.ViewModels;

namespace Hospital_Management.Validations.Appointments;

public class AppointmentCreateVMValidator : AbstractValidator<AppointmentCreateVM>
{
    public AppointmentCreateVMValidator()
    {
        RuleFor(x => x.AppointmentDate)
            .GreaterThanOrEqualTo(DateTime.Today).WithMessage("Görüş tarixi bu gün və ya daha sonrakı gün olmalıdır.");

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status boş ola bilməz.")
            .MaximumLength(50).WithMessage("Status 50 simvoldan çox ola bilməz.");

        RuleFor(x => x.DoctorId)
            .NotEmpty().WithMessage("Həkim seçilməlidir.");

        RuleFor(x => x.PatientId)
            .NotEmpty().WithMessage("Pasiyent seçilməlidir.");
    }
}