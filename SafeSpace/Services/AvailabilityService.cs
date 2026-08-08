using SafeSpace.Models;

namespace SafeSpace.Services;

public class AvailabilityService
{
    private readonly Dictionary<string, int> _roomInventory = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Deluxe Suite"] = 3,
        ["Signature Loft"] = 2,
        ["Garden Villa"] = 1
    };

    public Task<AvailabilityOutcome> CheckAvailabilityAsync(BookingRequest request, CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
        {
            var roomType = string.IsNullOrWhiteSpace(request.RoomType) ? "Deluxe Suite" : request.RoomType;
            if (!_roomInventory.ContainsKey(roomType))
            {
                _roomInventory[roomType] = 1;
            }

            var remaining = _roomInventory[roomType];
            var isAvailable = remaining > 0;
            _roomInventory[roomType] = Math.Max(0, remaining - 1);

            return new AvailabilityOutcome
            {
                IsAvailable = isAvailable,
                RemainingRooms = Math.Max(0, remaining - 1),
                Summary = isAvailable
                    ? $"{roomType} has availability for the requested dates."
                    : $"{roomType} is fully booked for the selected stay."
            };
        }, cancellationToken);
    }
}

public class AvailabilityOutcome
{
    public bool IsAvailable { get; set; }
    public int RemainingRooms { get; set; }
    public string Summary { get; set; } = "";
}
