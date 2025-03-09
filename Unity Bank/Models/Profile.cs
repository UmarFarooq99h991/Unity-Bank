using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Unity_Bank.Models
{
    public class Profile
    {
        [Key]
        public int? ProfileId { get; set; }

        [Required]
        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        [Required]
        public string? FullName { get; set; }

        [Required]
        [Phone]
        public string? PhoneNumber { get; set; }

        [Required]
        public string? Address { get; set; }

        public string ProfilePicturePath { get; set; } = "https://cdn-icons-png.flaticon.com/512/149/149071.png";

        // 🆕 New Fields
        [Required]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        public string? Gender { get; set; } // Male, Female, Other

        [Required]
        public string? Nationality { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
