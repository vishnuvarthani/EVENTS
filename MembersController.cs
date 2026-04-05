using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartEventManagement.Models.ViewModels;
using SmartEventManagement.Services;

namespace SmartEventManagement.Controllers;

[Authorize]
public class MembersController : Controller
{
    private readonly ICommunityEventService _service;

    public MembersController(ICommunityEventService service)
    {
        _service = service;
    }

    public IActionResult Dashboard()
    {
        var memberId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var member = _service.GetMember(memberId);
        if (member is null)
        {
            return RedirectToAction("Login", "Account");
        }

        var recommendations = _service.GetUpcomingEvents()
            .Where(e => member.Preferences.Contains(e.Category))
            .Take(3)
            .ToList();

        var model = new MemberDashboardViewModel
        {
            Member = member,
            Bookings = _service.GetBookingsForMember(memberId).ToList(),
            Reviews = _service.GetReviewsForMember(memberId).ToList(),
            RecommendedEvents = recommendations
        };

        return View(model);
    }

    [HttpGet]
    public IActionResult Review(int eventId)
    {
        var memberId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        if (!_service.CanReviewEvent(eventId, memberId))
        {
            TempData["ErrorMessage"] = "You can review an event only after booking it, and only once.";
            return RedirectToAction(nameof(Dashboard));
        }

        var communityEvent = _service.GetEvent(eventId);
        if (communityEvent is null)
        {
            return NotFound();
        }

        return View(new SubmitReviewViewModel
        {
            EventId = eventId,
            EventName = communityEvent.Name
        });
    }

    [HttpPost]
    public IActionResult Review(SubmitReviewViewModel model)
    {
        var memberId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var review = _service.SubmitReview(model, memberId, User.Identity?.Name ?? "Member", out var errorMessage);
        if (review is null)
        {
            ModelState.AddModelError(string.Empty, errorMessage);
            return View(model);
        }

        TempData["SuccessMessage"] = "Your review has been published.";
        return RedirectToAction(nameof(Dashboard));
    }
}
