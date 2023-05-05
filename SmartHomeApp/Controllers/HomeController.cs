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
        public async Task<IActionResult> Index(DeviceFilterViewModel filterViewModel)
        {
            var devices = _context.Devices
                .Include(d => d.Model)
                .Include(d => d.Status)
                .AsQueryable();

            if (!string.IsNullOrEmpty(filterViewModel.SearchString))
            {
                devices = devices.Where(d => d.DeviceName.Contains(filterViewModel.SearchString) || d.Location.Contains(filterViewModel.SearchString));
            }

            if (filterViewModel.StatusId != null)
            {
                devices = devices.Where(d => d.StatusId == filterViewModel.StatusId);
            }

            if (filterViewModel.InstallationDate != null)
            {
                devices = devices.Where(d => d.InstallationDate.Value.Date == filterViewModel.InstallationDate.Value.Date);
            }

            ViewBag.StatusList = new SelectList(_context.DeviceStatuses, "StatusId", "StatusName");

            var deviceList = await devices.ToListAsync();
            var viewModel = (devices: (IEnumerable<Device>)deviceList, filter: filterViewModel);
            return View(viewModel);
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

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = await _context.Devices
                .Include(d => d.Model) // Include DeviceModels
                .Include(d => d.Status) // Include DeviceStatus
                .Include(d => d.SecurityEvents) // Include SecurityEvents
                .Include(d => d.UserDevicePermissions) // Include UserDevicePermissions
                    .ThenInclude(udp => udp.User) // Include User
                .FirstOrDefaultAsync(m => m.DeviceId == id);
            if (device == null)
            {
                return NotFound();
            }

            return View(device);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = await _context.Devices.FindAsync(id);
            if (device == null)
            {
                return NotFound();
            }

            ViewBag.StatusList = new SelectList(_context.DeviceStatuses, "StatusId", "StatusName");
            ViewBag.ModelList = new SelectList(_context.DeviceModels, "ModelId", "ModelName");
            return View(device);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Device device)
        {
            if (id != device.DeviceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(device);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeviceExists(device.DeviceId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.StatusList = new SelectList(_context.DeviceStatuses, "StatusId", "StatusName");
            ViewBag.ModelList = new SelectList(_context.DeviceModels, "ModelId", "ModelName");
            return View(device);
        }

        private bool DeviceExists(int id)
        {
            return _context.Devices.Any(e => e.DeviceId == id);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = await _context.Devices
                .Include(d => d.Status)
                .FirstOrDefaultAsync(m => m.DeviceId == id);

            if (device == null)
            {
                return NotFound();
            }

            if (device.Status?.StatusName != "Работает")
            {
                _context.Devices.Remove(device);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction("Error", new { message = "Невозможно удалить устройство со статусом 'Работает'" });
            }
        }
        public IActionResult Error(string message)
        {
            ViewBag.ErrorMessage = message;
            return View();
        }


    }
}