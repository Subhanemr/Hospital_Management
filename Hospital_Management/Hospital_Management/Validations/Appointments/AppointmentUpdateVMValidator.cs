﻿using FluentValidation;
using Hospital_Management.ViewModels;

namespace Hospital_Management.Validations.Appointments;

public class AppointmentUpdateVMValidator : AbstractValidator<AppointmentCreateVM>
{
    public AppointmentUpdateVMValidator()
    {
        RuleFor(x => x.AppointmentDate)
            .GreaterThanOrEqualTo(DateTime.Today).WithMessage("Görüş tarixi bu gün və ya daha sonrakı gün olmalıdır.");

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status boş ola bilməz.");

        RuleFor(x => x.DoctorId)
            .NotEmpty().WithMessage("Həkim seçilməlidir.");

        RuleFor(x => x.PatientId)
            .NotEmpty().WithMessage("Pasiyent seçilməlidir.");
    }
}
