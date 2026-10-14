namespace Arcadia.Api.Contracts;

public record Item(long id, int quantity);

public record InventoryResponse(List<Item> items);