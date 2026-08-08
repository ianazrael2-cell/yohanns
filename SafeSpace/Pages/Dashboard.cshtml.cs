using Microsoft.AspNetCore.Mvc.RazorPages;
using SafeSpace.Models;
using SafeSpace.Services;

namespace SafeSpace.Pages;

public class DashboardModel : PageModel
{
    private readonly StaySureWorkflowService _workflowService;

    public DashboardModel(StaySureWorkflowService workflowService)
    {
        _workflowService = workflowService;
    }

    public IReadOnlyList<BookingRecord> Bookings { get; private set; } = Array.Empty<BookingRecord>();

    public void OnGet()
    {
        Bookings = _workflowService.Bookings;
    }
}
