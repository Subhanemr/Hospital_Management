namespace Hospital_Management.ViewModels;

public record MedicalCardIncludeVM : BaseEntityVM
{
    public string? DiseaseHistory { get; set; }
    public string? LabResults { get; set; }

    public string? PatientId { get; set; }
}
