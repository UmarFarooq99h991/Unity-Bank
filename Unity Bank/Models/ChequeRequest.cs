using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Unity_Bank.Models
{
    public class ChequeRequest
    {
        [Key]
        public int RequestId { get; set; }

        [ForeignKey("BankAccount")]
        public int AccountId { get; set; }
        public BankAccount BankAccount { get; set; }

        [Required]
        public string RequestType { get; set; } // "New Cheque Book" or "Stop Payment"

        [Required]
        public DateTime RequestDate { get; set; } = DateTime.Now;

        // 🆕 Status Field (Pending, Approved, Rejected)
        [Required]
        public string Status { get; set; } = "Pending";
        public bool IsProcessed { get; set; } = false;
        // 🆕 Processed Date (Nullable, updated when approved/rejected)
        public DateTime? ProcessedDate { get; set; }
    }
}
