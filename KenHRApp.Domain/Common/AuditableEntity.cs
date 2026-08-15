using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Common;

/// <summary>Adds audit + optimistic concurrency + soft delete support.</summary>
public abstract class AuditableEntity : BaseEntity, ISoftDeletable
{
    #region MyRegion
    public string? CreatedBy { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTimeOffset? ModifiedDate { get; set; }
    public byte[]? RowVersion { get; set; }
    #endregion

    #region ISoftDeletable Interface Implementations
    public bool IsDeleted { get; set; }
    public string? DeletedBy { get; set; }
    public DateTimeOffset? DeletedDate { get; set; }
    #endregion
}