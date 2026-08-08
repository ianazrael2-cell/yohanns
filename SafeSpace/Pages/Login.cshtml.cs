using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SafeSpace.Pages;

public class LoginModel : PageModel
{
    [BindProperty]
    public LoginInput Input { get; set; } = new();

    public string? Message { get; private set; }

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        if (Input.Email == "owner@staysure.test" && Input.Password == "StaySure2026!")
        {
            Message = "Welcome back, Yohann. You are now signed in.";
            return RedirectToPage("/Booking");
        }

        Message = "Invalid credentials. Please try again.";
        return Page();
    }
}

public class LoginInput
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
