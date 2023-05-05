using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SmartHomeApp.Models;
using System.Diagnostics;

namespace SmartHomeApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly SmartHomeContext _context;

        public HomeController(SmartHomeContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var devices = await _context.Devices
                .Include(d => d.Model) // Include DeviceModels
                .Include(d => d.Status) // Include DeviceStatus
                .ToListAsync();
            return View(devices);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Create()
        {
            ViewBag.StatusList = new SelectList(_context.DeviceStatuses, "StatusId", "StatusName");
            ViewBag.ModelList = new SelectList(_context.DeviceModels, "ModelId", "ModelName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Device device)
        {
            if (ModelState.IsValid)
            {
                _context.Add(device);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(device);
        }

    }
}