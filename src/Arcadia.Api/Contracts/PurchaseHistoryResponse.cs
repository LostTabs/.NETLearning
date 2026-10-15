namespace Arcadia.Api.Contracts;

public record Purchase(long ItemId, int Cost, DateTime TimeStamp);

public record PurchaseHistoryResponse(List<Purchase> Purchases);