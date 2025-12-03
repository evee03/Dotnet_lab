using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Laboratorium_7.Models;
using Laboratorium_7.Data; 
using Laboratorium_7.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;

namespace Laboratorium_7.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ChinookDbContext _chinook;
    private readonly UserManager<ApplicationUser> _userManager;

    public HomeController(ILogger<HomeController> logger,
                          ChinookDbContext chinook,
                          UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _chinook = chinook;
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        var customers = _chinook.Customers.ToList();

        return View(customers);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [Authorize]
    public async Task<IActionResult> MyOrders()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
            return Challenge(); 

        var customerId = user.CustomerId;

        var orders = await _chinook.Invoices
                                   .Where(x => x.CustomerId == customerId)
                                   .ToListAsync();

        return View(orders);
    }

    [Authorize]
    public async Task<IActionResult> OrderDetails(int id)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Challenge();

        var invoice = await _chinook.Invoices
            .Include(i => i.InvoiceLines)
                .ThenInclude(il => il.Track)
            .FirstOrDefaultAsync(i => i.InvoiceId == id);

        if (invoice == null)
            return NotFound();

        if (invoice.CustomerId != user.CustomerId)
            return Forbid();

        return View(invoice);
    }

}
