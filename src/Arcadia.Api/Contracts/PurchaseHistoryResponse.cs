namespace Arcadia.Api.Contracts;

public record Purchases(long ItemId, int Cost, DateTime TimeStamp);

public record PurchaseHistoryResponse(List<Purchases>  Purchases);