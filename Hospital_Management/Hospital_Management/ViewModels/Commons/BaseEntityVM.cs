namespace Hospital_Management.ViewModels;

public abstract record BaseEntityVM
{
    public string Id { get; set; } = null!; 
    public bool IsDeleted { get; set; }
    public DateTime CreateAt { get; set; }
    public DateTime? UpdateAt { get; set; }
    public string CreatedById { get; set; } = null!;
    public string? UpdatedById { get; set; }
}
