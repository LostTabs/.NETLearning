namespace Arcadia.Api.Contracts;

public record CatalogueProduct(long Id, string Name, int Cost);

public record CatalogueResponse(List<CatalogueProduct> Items);
