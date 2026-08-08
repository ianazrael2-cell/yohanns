using SafeSpace.Models;

namespace SafeSpace.Services;

public class InvoiceService
{
    public Task<InvoiceOutcome> GenerateInvoiceAsync(BookingRequest request, CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
        {
            var checkIn = DateOnly.Parse(request.CheckIn);
            var checkOut = DateOnly.Parse(request.CheckOut);
            var nights = Math.Max(1, checkOut.DayNumber - checkIn.DayNumber);
            var amount = Math.Round(180m * nights, 2);
            var invoiceId = $"INV-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..6].ToUpperInvariant()}";

            return new InvoiceOutcome
            {
                InvoiceId = invoiceId,
                Amount = amount,
                Summary = $"Prepared an invoice for {nights} night(s) at the premium nightly rate."
            };
        }, cancellationToken);
    }
}

public class InvoiceOutcome
{
    public string InvoiceId { get; set; } = "";
    public decimal Amount { get; set; }
    public string Summary { get; set; } = "";
}
