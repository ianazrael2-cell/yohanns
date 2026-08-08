using Microsoft.AspNetCore.Mvc.RazorPages;
using SafeSpace.Models;
using SafeSpace.Services;

namespace SafeSpace.Pages;

public class IndexModel : PageModel
{
    private readonly StaySureWorkflowService _workflowService;

    public IndexModel(StaySureWorkflowService workflowService)
    {
        _workflowService = workflowService;
    }

    public IReadOnlyList<BookingRecord> RecentBookings { get; private set; } = Array.Empty<BookingRecord>();

    public void OnGet()
    {
        RecentBookings = _workflowService.Bookings.Take(3).ToList();
    }
}
