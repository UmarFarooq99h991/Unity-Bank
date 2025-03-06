using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Unity_Bank.Data;
using Unity_Bank.Models;

public class TransactionController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    public TransactionController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    [HttpGet]
    public async Task<IActionResult> Transfer()
    {
        ViewBag.Accounts = await _context.BankAccounts.ToListAsync();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> TransferFunds(int senderAccountId, int receiverAccountId, decimal amount)
    {
        if (amount <= 0)
        {
            TempData["Error"] = "Invalid transfer amount.";
            return RedirectToAction("Transfer");
        }

        var senderAccount = await _context.BankAccounts.FirstOrDefaultAsync(a => a.AccountId == senderAccountId);
        var receiverAccount = await _context.BankAccounts.FirstOrDefaultAsync(a => a.AccountId == receiverAccountId);

        if (senderAccount == null || receiverAccount == null)
        {
            TempData["Error"] = "Invalid account details.";
            return RedirectToAction("Transfer");
        }

        if (senderAccount.Balance < amount)
        {
            TempData["Error"] = "Insufficient balance.";
            return RedirectToAction("Transfer");
        }

        // Deduct from sender
        senderAccount.Balance -= amount;

        // Credit to receiver
        receiverAccount.Balance += amount;

        // Log transactions for both accounts
        var senderTransaction = new Transaction
        {
            TransactionType = "Debit",
            Amount = amount,
            AccountId = senderAccount.AccountId,
            TransactionDate = DateTime.Now
        };

        var receiverTransaction = new Transaction
        {
            TransactionType = "Credit",
            Amount = amount,
            AccountId = receiverAccount.AccountId,
            TransactionDate = DateTime.Now
        };

        _context.Transactions.Add(senderTransaction);
        _context.Transactions.Add(receiverTransaction);
        await _context.SaveChangesAsync();

        TempData["Success"] = "Transfer successful!";
        return RedirectToAction("Transfer");
    }
    public async Task<IActionResult> History()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            TempData["Error"] = "User not found. Please log in.";
            return RedirectToAction("Login", "Account");
        }

        var accounts = await _context.BankAccounts.Where(a => a.UserId == user.Id).Select(a => a.AccountId).ToListAsync();

        if (!accounts.Any())
        {
            TempData["Error"] = "No accounts found.";
            return View(new List<Transaction>()); // Return empty list
        }

        var transactions = await _context.Transactions
            .Where(t => accounts.Contains(t.AccountId))
            .Include(t => t.BankAccount)
            .Include(t => t.ToAccount)
            .OrderByDescending(t => t.TransactionDate)
            .ToListAsync();

        return View(transactions);
    }


}
