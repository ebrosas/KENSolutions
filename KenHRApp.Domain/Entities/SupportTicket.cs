using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Entities
{
    public class SupportTicket
    {
        #region Properties
        public Guid Id { get; private set; } = Guid.NewGuid();

        public string Subject { get; private set; } = null!;
        public string Requester { get; private set; } = null!;
        public string Description { get; private set; } = null!;

        public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;
        public ICollection<SupportTicketAttachment> Attachments { get; private set; }
            = new List<SupportTicketAttachment>();
        #endregion

        #region Constructor
        private SupportTicket() { } // EF

        public SupportTicket(string subject, string requester, string description)
        {
            Subject = subject;
            Requester = requester;
            Description = description;
        }
        #endregion

        #region Public Methods
        public void AddAttachment(SupportTicketAttachment attachment)
        {
            Attachments.Add(attachment);
        }
        #endregion
    }
}
