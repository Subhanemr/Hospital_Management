namespace Hospital_Management.ViewModels;

public record MedicalCardGetVM : BaseEntityVM
{
    public string? DiseaseHistory { get; set; }
    public string? LabResults { get; set; }

    public string? PatientId { get; set; }
    public PatientIncludeVM? Patient { get; set; }
}
