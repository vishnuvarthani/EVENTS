using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartEventManagement.Models.ViewModels;
using SmartEventManagement.Services;

namespace SmartEventManagement.Controllers;

public class AccountController : Controller
{
    private readonly ICommunityEventService _service;

    public AccountController(ICommunityEventService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View(new RegisterViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (_service.EmailExists(model.Email))
        {
            ModelState.AddModelError(nameof(model.Email), "That email is already registered.");
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var member = _service.RegisterMember(model);
        await SignInUser(member);
        TempData["SuccessMessage"] = "Welcome to the cultural council portal. Your member profile is ready.";
        return RedirectToAction("Dashboard", "Members");
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null, string role = "Member")
    {
        ViewBag.ReturnUrl = returnUrl;
        ViewBag.LoginRole = role;
        return View(new LoginViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null, string role = "Member")
    {
        ViewBag.ReturnUrl = returnUrl;
        ViewBag.LoginRole = role;

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = _service.ValidateMember(model.Email, model.Password, role);
        if (user is null)
        {
            ModelState.AddModelError(string.Empty, $"Invalid {role.ToLower()} email or password.");
            return View(model);
        }

        await SignInUser(user);
        var fallbackAction = user.Role == "Admin"
            ? Url.Action("Index", "Admin")
            : Url.Action("Dashboard", "Members");

        return LocalRedirect(string.IsNullOrWhiteSpace(returnUrl) ? fallbackAction! : returnUrl);
    }

    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    private async Task SignInUser(Models.Member member)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, member.Id.ToString()),
            new(ClaimTypes.Name, member.FullName),
            new(ClaimTypes.Email, member.Email),
            new(ClaimTypes.Role, member.Role)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
    }
}
