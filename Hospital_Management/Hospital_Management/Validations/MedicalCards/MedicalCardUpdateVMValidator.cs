using FluentValidation;
using Hospital_Management.ViewModels;

namespace Hospital_Management.Validations.MedicalCards;

public class MedicalCardUpdateVMValidator : AbstractValidator<MedicalCardCreateVM>
{
    public MedicalCardUpdateVMValidator()
    {
        RuleFor(x => x.DiseaseHistory)
            .NotEmpty().WithMessage("Xəstəlik tarixi tələb olunur.")
            .MaximumLength(1000).WithMessage("Xəstəlik tarixi 1000 simvoldan çox ola bilməz.");

        RuleFor(x => x.LabResults)
            .NotEmpty().WithMessage("Analiz nəticələri tələb olunur.")
            .MaximumLength(1000).WithMessage("Analiz nəticələri 1000 simvoldan çox ola bilməz.");

        RuleFor(x => x.PatientId)
            .NotEmpty().WithMessage("Pasiyent seçilməlidir.");
    }
}