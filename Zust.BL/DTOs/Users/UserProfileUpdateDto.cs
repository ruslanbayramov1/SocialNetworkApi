namespace Zust.BL.DTOs.Users;

public class UserProfileUpdateDto
{
    public DateTime? DateOfBirth { get; set; }
    public Guid? OccupationId { get; set; }
    public Guid? GenderId { get; set; }
    public Guid? RelationStatusId { get; set; }
    public Guid? BloodGroupId { get; set; }
    public Guid? LanguageId { get; set; }
    public string? BackupEmail { get; set; }
    public string? Website { get; set; }
    public string? Address { get; set; }
    public bool IsPrivate { get; set; } = false;
}
