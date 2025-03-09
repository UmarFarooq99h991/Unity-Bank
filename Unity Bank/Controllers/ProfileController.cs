using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Unity_Bank.Data;
using Unity_Bank.Models;

[Authorize]
public class ProfileController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public ProfileController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // ✅ GET: Profile/Details
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound();

        var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.UserId == user.Id);
        if (profile == null) return RedirectToAction(nameof(Create));

        return View(profile);
    }

    // ✅ GET: Profile/Create
    public IActionResult Create()
    {
        return View();
    }

    // ✅ POST: Profile/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Profile profile)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound();

        profile.UserId = user.Id;

        if (ModelState.IsValid)
        {
            _context.Profiles.Add(profile);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(profile);
    }

    // ✅ GET: Profile/Edit
    public async Task<IActionResult> Edit()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound();

        var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.UserId == user.Id);
        if (profile == null) return RedirectToAction(nameof(Create));

        return View(profile);
    }

    // ✅ POST: Profile/Edit
    // ✅ POST: Profile/Edit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Profile profile)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            Console.WriteLine("❌ User not found!");
            return NotFound();
        }

        var existingProfile = await _context.Profiles.FirstOrDefaultAsync(p => p.UserId == user.Id);
        if (existingProfile == null)
        {
            Console.WriteLine("❌ Profile not found!");
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            Console.WriteLine("✅ Before Update - Existing Profile Data:");
            Console.WriteLine($"   FullName: {existingProfile.FullName}");
            Console.WriteLine($"   PhoneNumber: {existingProfile.PhoneNumber}");
            Console.WriteLine($"   Address: {existingProfile.Address}");

            // ✅ Updating fields
            existingProfile.FullName = profile.FullName;
            existingProfile.PhoneNumber = profile.PhoneNumber;
            existingProfile.Address = profile.Address;
            existingProfile.DateOfBirth = profile.DateOfBirth;
            existingProfile.Gender = profile.Gender;
            existingProfile.Nationality = profile.Nationality;
            existingProfile.UpdatedAt = DateTime.Now;  // 🆕 Update timestamp

            Console.WriteLine("✅ After Update - New Profile Data:");
            Console.WriteLine($"   FullName: {existingProfile.FullName}");
            Console.WriteLine($"   PhoneNumber: {existingProfile.PhoneNumber}");
            Console.WriteLine($"   Address: {existingProfile.Address}");

            _context.Entry(existingProfile).State = EntityState.Modified;
            _context.Profiles.Update(existingProfile);

            int changes = await _context.SaveChangesAsync();
            Console.WriteLine($"✅ Changes Saved: {changes}");

            return RedirectToAction(nameof(Index));
        }

        Console.WriteLine("❌ ModelState is invalid!");

        var errors = ModelState.Values.SelectMany(v => v.Errors);
        foreach (var error in errors)
        {
            Console.WriteLine($"❌ Model Error: {error.ErrorMessage}");
        }

        return View(profile);
    }


}
