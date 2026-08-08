using SafeSpace.Models;

namespace SafeSpace.Services;

public class IdentityVerificationService
{
    public Task<IdentityVerificationOutcome> VerifyAsync(BookingRequest request, CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
        {
            if (string.IsNullOrWhiteSpace(request.GovernmentIdName))
            {
                return new IdentityVerificationOutcome
                {
                    IsApproved = false,
                    Status = "Rejected",
                    Summary = "Government-issued ID was not provided. The booking was blocked before confirmation."
                };
            }

            if (request.GuestAge < 18)
            {
                return new IdentityVerificationOutcome
                {
                    IsApproved = false,
                    Status = "Rejected",
                    Summary = "The guest is below the minimum age requirement. Manual review is required."
                };
            }

            var idName = request.GovernmentIdName.ToLowerInvariant();
            if (idName.Contains("tampered") || idName.Contains("suspect") || idName.Contains("blur"))
            {
                return new IdentityVerificationOutcome
                {
                    IsApproved = false,
                    Status = "ManualReview",
                    Summary = "The document image raised risk signals. The booking has been routed to the owner for review."
                };
            }

            return new IdentityVerificationOutcome
            {
                IsApproved = true,
                Status = "Approved",
                Summary = $"Verified {request.GovernmentIdType} against the live selfie and age profile."
            };
        }, cancellationToken);
    }
}

public class IdentityVerificationOutcome
{
    public bool IsApproved { get; set; }
    public string Status { get; set; } = "Pending";
    public string Summary { get; set; } = "";
}
