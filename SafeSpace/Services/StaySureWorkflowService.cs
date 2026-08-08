using SafeSpace.Models;

namespace SafeSpace.Services;

public class StaySureWorkflowService
{
    private readonly IdentityVerificationService _identityVerificationService;
    private readonly AvailabilityService _availabilityService;
    private readonly InvoiceService _invoiceService;
    private readonly PaymentService _paymentService;
    private readonly ChannelSyncService _channelSyncService;
    private readonly List<BookingRecord> _bookings = new();

    public StaySureWorkflowService(
        IdentityVerificationService identityVerificationService,
        AvailabilityService availabilityService,
        InvoiceService invoiceService,
        PaymentService paymentService,
        ChannelSyncService channelSyncService)
    {
        _identityVerificationService = identityVerificationService;
        _availabilityService = availabilityService;
        _invoiceService = invoiceService;
        _paymentService = paymentService;
        _channelSyncService = channelSyncService;
    }

    public IReadOnlyList<BookingRecord> Bookings => _bookings;

    public async Task<BookingResult> ProcessBookingAsync(BookingRequest request, CancellationToken cancellationToken = default)
    {
        var bookingId = Guid.NewGuid().ToString("N")[..8].ToUpperInvariant();
        var identityTask = _identityVerificationService.VerifyAsync(request, cancellationToken);
        var availabilityTask = _availabilityService.CheckAvailabilityAsync(request, cancellationToken);
        var invoiceTask = _invoiceService.GenerateInvoiceAsync(request, cancellationToken);
        var paymentTask = _paymentService.CreatePaymentLinkAsync(request, cancellationToken);
        var channelTask = _channelSyncService.BlockDatesAsync(request, cancellationToken);

        await Task.WhenAll(identityTask, availabilityTask, invoiceTask, paymentTask, channelTask);

        var verification = await identityTask;
        var availability = await availabilityTask;
        var invoice = await invoiceTask;
        var payment = await paymentTask;
        var channel = await channelTask;

        BookingStatus status;
        string message;
        if (!verification.IsApproved)
        {
            status = verification.Status == "ManualReview" ? BookingStatus.ManualReview : BookingStatus.Rejected;
            message = verification.Summary;
        }
        else if (!availability.IsAvailable)
        {
            status = BookingStatus.ManualReview;
            message = availability.Summary;
        }
        else
        {
            status = BookingStatus.Confirmed;
            message = "The staycation booking is confirmed and ready for the guest to complete the deposit.";
        }

        var result = new BookingResult
        {
            BookingId = bookingId,
            Status = status,
            Message = message,
            VerificationSummary = verification.Summary,
            AvailabilitySummary = availability.Summary,
            ChannelSummary = channel.Summary,
            Amount = invoice.Amount,
            PaymentLink = payment.PaymentLink,
            InvoiceId = invoice.InvoiceId,
            IsConfirmed = status == BookingStatus.Confirmed
        };

        _bookings.Insert(0, new BookingRecord
        {
            BookingId = bookingId,
            GuestName = request.GuestName,
            RoomType = request.RoomType,
            CreatedAt = DateTime.UtcNow,
            Status = status,
            Summary = message,
            Amount = invoice.Amount,
            PaymentLink = payment.PaymentLink,
            Source = request.Source
        });

        return result;
    }
}
