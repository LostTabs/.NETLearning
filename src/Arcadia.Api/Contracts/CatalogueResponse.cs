namespace Arcadia.Api.Contracts;

public record CatalogueProduct(long ItemId, string Name, int Cost);

public record CatalogueResponse(List<CatalogueProduct> Products);
