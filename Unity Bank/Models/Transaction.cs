using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Unity_Bank.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }

        [Required]
        public string TransactionType { get; set; } // Deposit, Withdrawal, Transfer

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        // ✅ Primary Account (Sender for transfers, or main account for deposits)
        [Required]
        public int AccountId { get; set; }

        [ForeignKey("AccountId")]
        public BankAccount BankAccount { get; set; }

        // ✅ Destination Account (Nullable for non-transfer transactions)
        public int? ToAccountId { get; set; }

        [ForeignKey("ToAccountId")]
        public BankAccount? ToAccount { get; set; }

        // ✅ Ensure Date Exists
        [Required]
        public DateTime TransactionDate { get; set; } = DateTime.Now;

        // ✅ Status Field Added
        [Required]
        public string Status { get; set; } = "Pending"; // Success, Failed, Pending
    }
}
