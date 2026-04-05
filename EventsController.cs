using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartEventManagement.Models.ViewModels;
using SmartEventManagement.Services;

namespace SmartEventManagement.Controllers;

public class EventsController : Controller
{
    private readonly ICommunityEventService _service;

    public EventsController(ICommunityEventService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult Index([FromQuery] EventSearchViewModel search)
    {
        return View(_service.SearchEvents(search));
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
        var communityEvent = _service.GetEvent(id);
        if (communityEvent is null)
        {
            return NotFound();
        }

        return View(communityEvent);
    }

    [Authorize]
    [HttpGet]
    public IActionResult Book(int id)
    {
        var communityEvent = _service.GetEvent(id);
        if (communityEvent is null)
        {
            return NotFound();
        }

        var model = new BookTicketViewModel
        {
            EventId = communityEvent.Id,
            EventName = communityEvent.Name,
            EventDate = communityEvent.EventDate,
            Venue = communityEvent.Venue,
            SeatOptions = communityEvent.SeatOptions
        };

        return View(model);
    }

    [Authorize]
    [HttpPost]
    public IActionResult Book(BookTicketViewModel model)
    {
        var communityEvent = _service.GetEvent(model.EventId);
        if (communityEvent is null)
        {
            return NotFound();
        }

        model.EventName = communityEvent.Name;
        model.EventDate = communityEvent.EventDate;
        model.Venue = communityEvent.Venue;
        model.SeatOptions = communityEvent.SeatOptions;

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var memberId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var booking = _service.CreateBooking(model.EventId, memberId, model.SeatType, model.Quantity, out var errorMessage);

        if (booking is null)
        {
            ModelState.AddModelError(string.Empty, errorMessage);
            return View(model);
        }

        TempData["SuccessMessage"] = $"Booking confirmed for {booking.Quantity} {booking.SeatType} ticket(s).";
        return RedirectToAction(nameof(Confirmation), new { id = booking.Id });
    }

    [Authorize]
    public IActionResult Confirmation(int id)
    {
        var memberId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var booking = _service.GetBookingsForMember(memberId).FirstOrDefault(b => b.Id == id);
        if (booking is null)
        {
            return NotFound();
        }

        return View(booking);
    }
}
