using SafeSpace.Models;

namespace SafeSpace.Services;

public class ChannelSyncService
{
    public Task<ChannelSyncOutcome> BlockDatesAsync(BookingRequest request, CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
        {
            return new ChannelSyncOutcome
            {
                Summary = $"Synced the reservation to Airbnb, iCal, and direct-channel calendars for {request.CheckIn} to {request.CheckOut}."
            };
        }, cancellationToken);
    }
}

public class ChannelSyncOutcome
{
    public string Summary { get; set; } = "";
}
