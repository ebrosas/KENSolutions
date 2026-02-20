using System.ComponentModel.DataAnnotations;

namespace KenHRApp.Application.DTOs
{
    public class SubmitTicketDTO
    {
        #region Properties
        [Required]
        [MaxLength(200)]
        public string Subject { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string Requester { get; set; } = string.Empty;

        [Required]
        [MaxLength(2000)]
        public string Description { get; set; } = string.Empty;
        #endregion
    }
}
