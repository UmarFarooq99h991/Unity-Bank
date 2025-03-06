using System.ComponentModel.DataAnnotations;

namespace Unity_Bank.Models
{
    public class TransferViewModel
    {
        [Required]
        public string FromAccount { get; set; }

        [Required]
        public string ToAccount { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string TransactionPassword { get; set; }
    }
}
