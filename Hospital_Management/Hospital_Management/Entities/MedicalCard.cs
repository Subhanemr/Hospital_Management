namespace Hospital_Management.Entities;

public class MedicalCard : BaseEntity
{
    public string DiseaseHistory { get; set; } = null!;
    public string LabResults { get; set; } = null!;

    public string PatientId { get; set; }
    public Patient Patient { get; set; } = null!;
}
