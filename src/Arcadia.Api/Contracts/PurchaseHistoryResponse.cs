namespace Arcadia.Api.Contracts;

public record Purchase(long PurchaseId, long ItemId, int Cost, DateTime Date);

public record PurchaseHistoryResponse(List<Purchase> Purchases);