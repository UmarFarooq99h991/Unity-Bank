using System.Collections.Generic;
using Unity_Bank.Models;

namespace Unity_Bank.Models
{
    public class AdminDashboardViewModel
    {
        public List<ApplicationUser> Users { get; set; }
        public List<BankAccount> Accounts { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}
