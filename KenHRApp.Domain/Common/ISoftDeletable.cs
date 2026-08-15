namespace KenHRApp.Domain.Common;

public interface ISoftDeletable
{
    bool IsDeleted { get; set; }
    string? DeletedBy { get; set; }
    DateTimeOffset? DeletedDate { get; set; }
}
