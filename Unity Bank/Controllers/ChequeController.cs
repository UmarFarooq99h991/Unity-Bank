using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Unity_Bank.Data;
using Unity_Bank.Models;

namespace Unity_Bank.Controllers
{
    [Authorize]
    public class ChequeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ChequeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ✅ GET: Request Cheque View
        public async Task<IActionResult> RequestCheque()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var accounts = await _context.BankAccounts
                .Where(a => a.UserId == user.Id.ToString())
                .ToListAsync();

            if (!accounts.Any())
            {
                TempData["Error"] = "No bank accounts found.";
                return RedirectToAction("Index", "Dashboard");
            }

            var viewModel = new ChequeRequestViewModel
            {
                Accounts = accounts
            };

            return View(viewModel); // ✅ Pass ViewModel instead of ViewBag
        }


        // ✅ POST: Submit Cheque Request
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RequestCheque(ChequeRequestViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var account = await _context.BankAccounts
                .FirstOrDefaultAsync(a => a.AccountId == model.AccountId && a.UserId == user.Id);

            if (account == null)
            {
                TempData["Error"] = "Invalid bank account.";
                return RedirectToAction("RequestCheque");
            }

            if (model.RequestType != "New Cheque Book" && model.RequestType != "Stop Payment")
            {
                TempData["Error"] = "Invalid request type.";
                return RedirectToAction("RequestCheque");
            }

            var chequeRequest = new ChequeRequest
            {
                AccountId = model.AccountId,
                RequestType = model.RequestType,
                Status = "Pending"
            };

            _context.ChequeRequests.Add(chequeRequest);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Cheque request submitted successfully.";
            return RedirectToAction("History", "Transaction");
        }


        // ✅ GET: View All Requests (for user)
        public async Task<IActionResult> MyRequests()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var requests = await _context.ChequeRequests
                .Include(r => r.BankAccount)
                .Where(r => r.BankAccount.UserId == user.Id.ToString())
                .ToListAsync();

            return View(requests);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminChequeRequests()
        {
            var requests = await _context.ChequeRequests
                .Include(r => r.BankAccount)
                .ToListAsync();

            return View(requests);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> ApproveRequest(int requestId)
        {
            var request = await _context.ChequeRequests.FindAsync(requestId);
            if (request == null) return NotFound();

            request.IsProcessed = true;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(AdminChequeRequests));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> RejectRequest(int requestId)
        {
            var request = await _context.ChequeRequests.FindAsync(requestId);
            if (request == null) return NotFound();

            _context.ChequeRequests.Remove(request);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(AdminChequeRequests));
        }
    }
}
