using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SmartEventManagement.Models;
using SmartEventManagement.Models.ViewModels;
using SmartEventManagement.Services;

namespace SmartEventManagement.Controllers;

public class HomeController : Controller
{
    private readonly ICommunityEventService _service;

    public HomeController(ICommunityEventService service)
    {
        _service = service;
    }

    public IActionResult Index()
    {
        var upcomingEvents = _service.GetUpcomingEvents();
        var model = new HomeIndexViewModel
        {
            FeaturedEvents = upcomingEvents.Take(3).ToList(),
            RecentReviews = upcomingEvents
                .SelectMany(e => e.Reviews.Select(r => new HomeReviewItemViewModel
                {
                    EventName = e.Name,
                    MemberName = r.MemberName,
                    Comment = r.Comment,
                    Rating = r.Rating,
                    SubmittedOn = r.SubmittedOn
                }))
                .OrderByDescending(r => r.SubmittedOn)
                .Take(3)
                .ToList(),
            MemberCount = _service.MemberCount,
            BookingCount = _service.BookingCount,
            InquiryCount = _service.InquiryCount,
            MemberName = User.Identity?.IsAuthenticated == true ? User.Identity.Name : null
        };

        return View(model);
    }

    public IActionResult About()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
