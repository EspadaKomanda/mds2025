namespace PurpleHackBackend.Models.Database;

public class AuditableEntity
{ 
    public DateTimeOffset CreatedOn { get; set; }
    public string CreatedBy { get; set; }
    public DateTimeOffset UpdatedOn { get; set; }
    public string UpdatedBy { get; set; }
}