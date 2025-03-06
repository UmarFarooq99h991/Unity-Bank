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
        private readonly UserManager<ApplicationUser> _userManager; // ✅ Add this

        public ChequeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> RequestCheque()
        {
            var user = await _userManager.GetUserAsync(User); // ✅ Fix here
            var accounts = await _context.BankAccounts
                .Where(a => a.UserId == user.Id)
                .ToListAsync();

            ViewBag.Accounts = accounts;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RequestCheque(int AccountId, string RequestType)
        {
            var chequeRequest = new ChequeRequest
            {
                AccountId = AccountId,
                RequestType = RequestType
            };

            _context.ChequeRequests.Add(chequeRequest);
            await _context.SaveChangesAsync();

            return RedirectToAction("History", "Transaction");
        }
    }
}
