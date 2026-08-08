using Microsoft.AspNetCore.Mvc;
using SafeSpace.Models;
using SafeSpace.Services;

namespace SafeSpace.Controllers;

[ApiController]
[Route("api/webhooks")]
public class WebhooksController : ControllerBase
{
    private readonly StaySureWorkflowService _workflowService;

    public WebhooksController(StaySureWorkflowService workflowService)
    {
        _workflowService = workflowService;
    }

    [HttpPost("messenger")]
    public async Task<ActionResult<BookingResult>> Messenger([FromBody] BookingRequest request, CancellationToken cancellationToken)
    {
        request.Source = "Facebook Messenger";
        var result = await _workflowService.ProcessBookingAsync(request, cancellationToken);
        return Ok(result);
    }
}
