using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartEventManagement.Models.ViewModels;
using SmartEventManagement.Services;

namespace SmartEventManagement.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly ICommunityEventService _service;

    public AdminController(ICommunityEventService service)
    {
        _service = service;
    }

    public IActionResult Index()
    {
        var model = new AdminDashboardViewModel
        {
            Events = _service.GetAllEventsForAdmin().ToList(),
            Members = _service.GetAllMembers().ToList(),
            Inquiries = _service.GetAllInquiries().ToList()
        };

        return View(model);
    }

    [HttpGet]
    public IActionResult CreateEvent()
    {
        return View("EventForm", new AdminEventViewModel());
    }

    [HttpPost]
    public IActionResult CreateEvent(AdminEventViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("EventForm", model);
        }

        _service.CreateEvent(model);
        TempData["SuccessMessage"] = "New event created successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult EditEvent(int id)
    {
        var model = _service.GetAdminEvent(id);
        if (model is null)
        {
            return NotFound();
        }

        return View("EventForm", model);
    }

    [HttpPost]
    public IActionResult EditEvent(AdminEventViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("EventForm", model);
        }

        if (!_service.UpdateEvent(model))
        {
            return NotFound();
        }

        TempData["SuccessMessage"] = "Event updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult DeleteEvent(int id)
    {
        if (!_service.DeleteEvent(id))
        {
            return NotFound();
        }

        TempData["SuccessMessage"] = "Event deleted successfully.";
        return RedirectToAction(nameof(Index));
    }
}
