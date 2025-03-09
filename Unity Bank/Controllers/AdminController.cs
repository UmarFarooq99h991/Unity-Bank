//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System.Linq;
//using System.Threading.Tasks;
//using Unity_Bank.Data;
//using Unity_Bank.Models;

//[Authorize(Roles = "Admin")]  // Only admin can access this controller
//public class AdminController : Controller
//{
//    private readonly ApplicationDbContext _context;

//    public AdminController(ApplicationDbContext context)
//    {
//        _context = context;
//    }

//    // GET: Admin/Dashboard
//    public async Task<IActionResult> Dashboard()
//    {
//        var users = await _context.Users.ToListAsync();
//        var accounts = await _context.BankAccounts.ToListAsync();
//        var transactions = await _context.Transactions.ToListAsync();

//        var viewModel = new AdminDashboardViewModel
//        {
//            Users = users,
//            Accounts = accounts,
//            Transactions = transactions
//        };

//        return View(viewModel);
//    }

//    // GET: Admin/Users
//    public async Task<IActionResult> Users()
//    {
//        var users = await _context.Users.ToListAsync();
//        return View(users);
//    }

//    // Block or Unblock User
//    public async Task<IActionResult> ToggleUserStatus(string id)
//    {
//        var user = await _context.Users.FindAsync(id);
//        if (user == null)
//        {
//            return NotFound();
//        }

//        user.IsBlocked = !user.IsBlocked;
//        _context.Update(user);
//        await _context.SaveChangesAsync();

//        return RedirectToAction(nameof(Users));
//    }

//    // GET: Admin/Transactions
//    public async Task<IActionResult> Transactions()
//    {
//        var transactions = await _context.Transactions.OrderByDescending(t => t.TransactionDate).ToListAsync();
//        return View(transactions);
//    }
//    [Authorize(Roles = "Admin")]
//    public async Task<IActionResult> ApproveTransfers()
//    {
//        var pendingTransactions = _context.Transactions.Where(t => t.Status == "Pending Admin Approval").ToList();
//        return View(pendingTransactions);
//    }

//    [HttpPost]
//    [Authorize(Roles = "Admin")]
//    public async Task<IActionResult> ApproveTransfer(int transactionId)
//    {
//        var transaction = _context.Transactions.Find(transactionId);
//        if (transaction == null) return NotFound();

//        var senderAccount = _context.BankAccounts.Find(transaction.AccountId);
//        var recipientAccount = _context.BankAccounts.Find(transaction.ToAccountId);

//        senderAccount.Balance -= transaction.Amount;
//        recipientAccount.Balance += transaction.Amount;

//        transaction.Status = "Success";

//        await _context.SaveChangesAsync();
//        return RedirectToAction("ApproveTransfers");
//    }
//      [Authorize(Roles = "Admin")]
//    public class AdminController : Controller
//    {
//        private readonly ApplicationDbContext _context;

//        public AdminController(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<IActionResult> ApproveTransfers()
//        {
//            var pendingTransactions = await _context.Transactions
//                .Where(t => t.Status == "Pending")
//                .ToListAsync();

//            return View(pendingTransactions);
//        }

//        [HttpPost]
//        public async Task<IActionResult> ApproveTransfer(int transactionId)
//        {
//            var transaction = await _context.Transactions.FindAsync(transactionId);
//            if (transaction == null) return NotFound();

//            // Update transaction status
//            transaction.Status = "Approved";
//            await _context.SaveChangesAsync();

//            return RedirectToAction(nameof(ApproveTransfers));
//        }

//        [HttpPost]
//        public async Task<IActionResult> RejectTransfer(int transactionId)
//        {
//            var transaction = await _context.Transactions.FindAsync(transactionId);
//            if (transaction == null) return NotFound();

//            // Update transaction status
//            transaction.Status = "Rejected";
//            await _context.SaveChangesAsync();

//            return RedirectToAction(nameof(ApproveTransfers));
//        }
//    }

//}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Unity_Bank.Data;
using Unity_Bank.Models;

namespace Unity_Bank.Controllers
{
    [Authorize(Roles = "Admin")]  // Ensuring only Admin can access this controller
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ Admin Dashboard
        public async Task<IActionResult> Dashboard()
        {
            var users = await _context.Users.ToListAsync();
            var accounts = await _context.BankAccounts.ToListAsync();
            var transactions = await _context.Transactions.ToListAsync();

            var viewModel = new AdminDashboardViewModel
            {
                Users = users,
                Accounts = accounts,
                Transactions = transactions
            };

            return View(viewModel);
        }

        // ✅ View All Users
        public async Task<IActionResult> Users()
        {
            var users = await _context.Users.ToListAsync();
            return View(users);
        }

        // ✅ Block or Unblock a User
        public async Task<IActionResult> ToggleUserStatus(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            user.IsBlocked = !user.IsBlocked;
            _context.Update(user);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Users));
        }

        // ✅ View All Transactions
        public async Task<IActionResult> Transactions()
        {
            var transactions = await _context.Transactions.OrderByDescending(t => t.TransactionDate).ToListAsync();
            return View(transactions);
        }

        // ✅ List Pending Transactions for Approval
        public async Task<IActionResult> ApproveTransfers()
        {
            var pendingTransactions = await _context.Transactions
                .Where(t => t.Status == "Pending Admin Approval")
                .ToListAsync();

            return View(pendingTransactions);
        }

        // ✅ Approve a Transaction
        //[HttpPost]
        //public async Task<IActionResult> ApproveTransfer(int transactionId)
        //{
        //    var transaction = await _context.Transactions.FindAsync(transactionId);
        //    if (transaction == null) return NotFound();

        //    var senderAccount = await _context.BankAccounts.FindAsync(transaction.AccountId);
        //    var recipientAccount = await _context.BankAccounts.FindAsync(transaction.ToAccountId);

        //    if (senderAccount == null || recipientAccount == null)
        //        return BadRequest("Invalid Accounts");

        //    // Deduct from sender and add to recipient
        //    senderAccount.Balance -= transaction.Amount;
        //    recipientAccount.Balance += transaction.Amount;

        //    transaction.Status = "Success";

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(ApproveTransfers));
        //}
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveTransfer(int transactionId)
        {
            var transaction = await _context.Transactions.FindAsync(transactionId);
            if (transaction == null) return NotFound();

            var senderAccount = await _context.BankAccounts.FindAsync(transaction.AccountId);
            var recipientAccount = await _context.BankAccounts.FindAsync(transaction.ToAccountId);

            if (senderAccount.Balance < transaction.Amount)
            {
                transaction.Status = "Failed: Insufficient Funds";
            }
            else
            {
                senderAccount.Balance -= transaction.Amount;
                recipientAccount.Balance += transaction.Amount;
                transaction.Status = "Approved";
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ApproveTransfers));
        }

        //// ✅ Reject a Transaction
        //[HttpPost]
        //public async Task<IActionResult> RejectTransfer(int transactionId)
        //{
        //    var transaction = await _context.Transactions.FindAsync(transactionId);
        //    if (transaction == null) return NotFound();

        //    transaction.Status = "Rejected";
        //    await _context.SaveChangesAsync();

        //    return RedirectToAction(nameof(ApproveTransfers));
        //}
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RejectTransfer(int transactionId)
        {
            var transaction = await _context.Transactions.FindAsync(transactionId);
            if (transaction == null) return NotFound();

            transaction.Status = "Rejected";

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ApproveTransfers));
        }

    }
}
