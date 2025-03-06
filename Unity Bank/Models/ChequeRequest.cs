using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Unity_Bank.Models;

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

        public bool IsProcessed { get; set; } = false;
    }
}
