using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Unity_Bank.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string? TransactionPassword { get; set; } = "Default@123"; // Separate from login password
        public string Role { get; set; } = "User"; // Default role
        public string FullName { get; set; } = "Default User";
        public bool IsBlocked { get; set; } = false;

        public int FailedTransactionAttempts { get; set; } = 0; // Track wrong attempts
        public bool IsTransactionLocked { get; set; } = false;  // Lock if attempts exceed 5
    }
}
