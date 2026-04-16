using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Entities.Workflow
{
    public class WorkflowApprovalRole
    {
        #region Properties
        public int ApprovalRoleId { get; set; }         // Identity column  

        [Column(TypeName = "varchar(50)")]
        public string ApprovalGroupCode { get; set; } = default!;

        [Column(TypeName = "varchar(200)")]
        public string ApprovalGroupDesc { get; set; } = default!;

        [Column(TypeName = "varchar(300)")]
        public string? Remarks { get; set; }
        
        public int AssigneeEmpNo { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string? AssigneEmpName { get; set; } = null;

        [Column(TypeName = "varchar(50)")]
        public string? AssigneEmail { get; set; } = null;

        public int? SubstituteEmpNo { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string? SubstituteEmpName { get; set; } = null;

        [Column(TypeName = "varchar(50)")]
        public string? SubstituteEmail { get; set; } = null;

        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }

        public int? CreatedBy { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? CreatedUserID { get; set; } = null;

        [Column(TypeName = "datetime")]
        public DateTime? LastUpdatedDate { get; set; }

        public int? LastUpdatedBy { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? LastUpdatedUserID { get; set; } = null;

        [Column(TypeName = "tinyint")]
        [Comment("0 = Assignee Employee; 1 = Supervisor; 2 = Superintendent; 3 = Cost Center Manager")]
        public byte GroupType { get; set; }
        #endregion
    }
}
