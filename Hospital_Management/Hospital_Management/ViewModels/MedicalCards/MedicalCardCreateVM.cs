namespace Hospital_Management.ViewModels;

public record MedicalCardCreateVM
{
    public string DiseaseHistory { get; set; } = null!;
    public string LabResults { get; set; } = null!;

    public string PatientId { get; set; } = null!;
}
