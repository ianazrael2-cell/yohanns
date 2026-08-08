using SafeSpace.Models;

namespace SafeSpace.Services;

public class PaymentService
{
    public Task<PaymentOutcome> CreatePaymentLinkAsync(BookingRequest request, CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
        {
            var paymentId = Guid.NewGuid().ToString("N")[..8].ToUpperInvariant();
            var paymentLink = $"https://pay.staysure.test/checkout/{paymentId}";

            return new PaymentOutcome
            {
                PaymentLink = paymentLink,
                Summary = "Generated a secure payment link for the deposit and balance."
            };
        }, cancellationToken);
    }
}

public class PaymentOutcome
{
    public string PaymentLink { get; set; } = "";
    public string Summary { get; set; } = "";
}
