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

        public decimal Amount { get; set; }

        // ✅ Primary Account
        [Required]
        public int AccountId { get; set; }

        [ForeignKey("AccountId")]
        public BankAccount BankAccount { get; set; }

        // ✅ Destination Account (Nullable for non-transfer transactions)
        public int? ToAccountId { get; set; }

        [ForeignKey("ToAccountId")]
        public BankAccount? ToAccount { get; set; }

        public DateTime TransactionDate { get; set; } = DateTime.Now;
    }
}
