namespace Arcadia.Api.Contracts;

public record PurchaseResponse(long PurchaseId, long ItemId, int Cost, int NewBalance);
