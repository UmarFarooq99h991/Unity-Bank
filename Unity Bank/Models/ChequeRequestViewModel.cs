namespace Unity_Bank.Models
{
    public class ChequeRequestViewModel
    {
        public int AccountId { get; set; }
        public string RequestType { get; set; } // "New Cheque Book" or "Stop Payment"
        public List<BankAccount> Accounts { get; set; } // ✅ To display the user's accounts
    }
}
 