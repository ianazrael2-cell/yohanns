using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SafeSpace.Models;
using SafeSpace.Services;

namespace SafeSpace.Pages;

public class BookingModel : PageModel
{
    private readonly StaySureWorkflowService _workflowService;

    public BookingModel(StaySureWorkflowService workflowService)
    {
        _workflowService = workflowService;
    }

    [BindProperty]
    public BookingRequest Input { get; set; } = new();

    [BindProperty]
    public IFormFile? GovernmentId { get; set; }

    public BookingResult? LastResult { get; private set; }
    public string? Feedback { get; private set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (string.IsNullOrWhiteSpace(Input.GuestName) || string.IsNullOrWhiteSpace(Input.GuestEmail))
        {
            Feedback = "Please add the guest name and email before we continue.";
            return Page();
        }

        if (Input.GuestAge < 18)
        {
            Feedback = "The guest must be 18 or older to continue with verification.";
            return Page();
        }

        Input.GovernmentIdName = GovernmentId?.FileName ?? Input.GovernmentIdName;
        Input.Source = "WebForm";

        LastResult = await _workflowService.ProcessBookingAsync(Input);
        Feedback = LastResult.IsConfirmed
            ? "The booking is confirmed and ready for payment."
            : "The workflow completed, but review or a follow-up is needed.";

        return Page();
    }
}
