using Microsoft.AspNetCore.Mvc;
using SmartEventManagement.Models.ViewModels;
using SmartEventManagement.Services;

namespace SmartEventManagement.Controllers;

public class InquiryController : Controller
{
    private readonly ICommunityEventService _service;

    public InquiryController(ICommunityEventService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new GuestInquiryViewModel());
    }

    [HttpPost]
    public IActionResult Create(GuestInquiryViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        _service.SubmitInquiry(model);
        TempData["SuccessMessage"] = "Your inquiry has been sent to the council team.";
        return RedirectToAction("Index", "Home");
    }
}
