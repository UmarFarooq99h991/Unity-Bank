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
    public async Task<IActionResult> TransferFunds()
    {
        ViewBag.Accounts = await _context.BankAccounts.ToListAsync();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> TransferFunds(string RecipientAccountNumber, decimal Amount, string TransactionPassword)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        // Check if account is locked
        if (user.IsTransactionLocked)
        {
            return View("Error", new ErrorViewModel { Message = "Your transaction access is locked due to multiple failed attempts." });

        }

        var senderAccount = _context.BankAccounts.FirstOrDefault(a => a.UserId == user.Id);
        var recipientAccount = _context.BankAccounts.FirstOrDefault(a => a.AccountNumber == RecipientAccountNumber);

        // Validate recipient
        if (recipientAccount == null)
        {
            return View("Error", new ErrorViewModel { Message = "Recipient account not found." });
        }

        // Check balance
        if (senderAccount.Balance < Amount)
        {
            return View("Error", new ErrorViewModel { Message = "Insufficient balance." });
        }

        // Validate transaction password
        if (user.TransactionPassword != TransactionPassword)
        {
            user.FailedTransactionAttempts++;

            if (user.FailedTransactionAttempts >= 5)
            {
                user.IsTransactionLocked = true;
            }

            await _userManager.UpdateAsync(user);
            return View("Error", new ErrorViewModel { Message = "Invalid transaction password." });

        }

        // Reset failed attempts
        user.FailedTransactionAttempts = 0;
        await _userManager.UpdateAsync(user);

        // Check daily limit
        var todayTotal = _context.Transactions
            .Where(t => t.AccountId == senderAccount.AccountId && t.TransactionDate.Date == DateTime.Today)
            .Sum(t => t.Amount);

        if ((todayTotal + Amount) > 50000)
        {
            return View("Error", new ErrorViewModel { Message = "Daily transfer limit exceeded." });

        }

        // Check if admin approval is needed
        if (Amount >= 100000)
        {
            var transaction = new Transaction
            {
                AccountId = senderAccount.AccountId,
                ToAccountId = recipientAccount.AccountId,
                Amount = Amount,
                TransactionType = "Transfer",
                Status = "Pending Admin Approval"
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return View("Success", "Transfer submitted for admin approval.");
        }

        // Process transfer
        senderAccount.Balance -= Amount;
        recipientAccount.Balance += Amount;

        var successfulTransaction = new Transaction
        {
            AccountId = senderAccount.AccountId,
            ToAccountId = recipientAccount.AccountId,
            Amount = Amount,
            TransactionType = "Transfer",
            Status = "Success"
        };

        _context.Transactions.Add(successfulTransaction);
        await _context.SaveChangesAsync();

        return View("Success", "Transfer completed successfully.");
    }

    //public async Task<IActionResult> TransferFunds(int senderAccountId, int receiverAccountId, decimal amount)
    //{

    //    if (amount <= 0)
    //    {
    //        TempData["Error"] = "Invalid transfer amount.";
    //        return RedirectToAction("Transfer");

    //    }

    //    var senderAccount = await _context.BankAccounts.FirstOrDefaultAsync(a => a.AccountId == senderAccountId);
    //    var receiverAccount = await _context.BankAccounts.FirstOrDefaultAsync(a => a.AccountId == receiverAccountId);

    //    if (senderAccount == null || receiverAccount == null)
    //    {
    //        TempData["Error"] = "Invalid account details.";
    //        return RedirectToAction("Transfer");
    //    }

    //    if (senderAccount.Balance < amount)
    //    {
    //        TempData["Error"] = "Insufficient balance.";
    //        return RedirectToAction("Transfer");
    //    }

    //    // Deduct from sender
    //    senderAccount.Balance -= amount;

    //    // Credit to receiver
    //    receiverAccount.Balance += amount;

    //    // Log transactions for both accounts
    //    var senderTransaction = new Transaction
    //    {
    //        TransactionType = "Debit",
    //        Amount = amount,
    //        AccountId = senderAccount.AccountId,
    //        TransactionDate = DateTime.Now
    //    };

    //    var receiverTransaction = new Transaction
    //    {
    //        TransactionType = "Credit",
    //        Amount = amount,
    //        AccountId = receiverAccount.AccountId,
    //        TransactionDate = DateTime.Now
    //    };

    //    _context.Transactions.Add(senderTransaction);
    //    _context.Transactions.Add(receiverTransaction);
    //    await _context.SaveChangesAsync();

    //    TempData["Success"] = "Transfer successful!";
    //    return RedirectToAction("Transfer");
    //}
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
