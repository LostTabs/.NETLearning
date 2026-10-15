namespace Arcadia.Api.Contracts;

public record Item(long Id, int Quantity);

public record InventoryResponse(List<Item> Items);