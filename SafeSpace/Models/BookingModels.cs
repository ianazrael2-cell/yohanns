namespace SafeSpace.Models;

public enum BookingStatus
{
    Confirmed,
    ManualReview,
    Rejected
}

public class BookingRequest
{
    public string GuestName { get; set; } = "";
    public string GuestEmail { get; set; } = "";
    public string RoomType { get; set; } = "Deluxe Suite";
    public string CheckIn { get; set; } = DateTime.Today.ToString("yyyy-MM-dd");
    public string CheckOut { get; set; } = DateTime.Today.AddDays(2).ToString("yyyy-MM-dd");
    public string GovernmentIdType { get; set; } = "Passport";
    public string GovernmentIdName { get; set; } = "";
    public string SelfieNote { get; set; } = "";
    public int GuestAge { get; set; } = 25;
    public string Source { get; set; } = "Messenger";
}

public class BookingRecord
{
    public string BookingId { get; set; } = "";
    public string GuestName { get; set; } = "";
    public string RoomType { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public BookingStatus Status { get; set; }
    public string StatusLabel => Status.ToString();
    public string Summary { get; set; } = "";
    public decimal Amount { get; set; }
    public string PaymentLink { get; set; } = "";
    public string Source { get; set; } = "";
}

public class BookingResult
{
    public string BookingId { get; set; } = "";
    public BookingStatus Status { get; set; }
    public string Message { get; set; } = "";
    public string VerificationSummary { get; set; } = "";
    public string AvailabilitySummary { get; set; } = "";
    public string ChannelSummary { get; set; } = "";
    public decimal Amount { get; set; }
    public string PaymentLink { get; set; } = "";
    public string InvoiceId { get; set; } = "";
    public bool IsConfirmed { get; set; }
}
