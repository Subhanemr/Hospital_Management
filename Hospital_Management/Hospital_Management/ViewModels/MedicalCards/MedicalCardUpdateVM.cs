namespace Hospital_Management.ViewModels;

public record MedicalCardUpdateVM
{
    public string Id { get; set; } = null!;
    public string DiseaseHistory { get; set; } = null!;
    public string LabResults { get; set; } = null!;

    public string PatientId { get; set; } = null!;
}
