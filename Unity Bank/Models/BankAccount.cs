using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Unity_Bank.Models
{
    public enum AccountTypeEnum
    {
        Savings,
        Checking
    }

    public class BankAccount
    {
        [Key]
        public int AccountId { get; set; }

        [Required]
        [StringLength(20)] // Limit Account Number length
        public string AccountNumber { get; set; }

        [Required]
        public String AccountType { get; set; } // Enum instead of string

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; } = 0; // Initialize Balance

        [Required]
        public string UserId { get; set; } // Foreign key for User

        public ApplicationUser ApplicationUser { get; set; } // Navigation property
    }
}
