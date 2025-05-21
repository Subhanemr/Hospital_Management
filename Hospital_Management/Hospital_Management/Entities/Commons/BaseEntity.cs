namespace Hospital_Management.Entities;

public abstract class BaseEntity
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public bool IsDeleted { get; set; }
    public DateTime CreateAt { get; set; }
    public DateTime? UpdateAt { get; set; }
    public string CreatedById { get; set; } = null!;
    public string? UpdatedById { get; set; }
}
