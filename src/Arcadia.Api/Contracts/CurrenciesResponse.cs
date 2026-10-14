namespace Arcadia.Api.Contracts;

public record Currency(long currency, int quantity);

public record CurrenciesResponse(List<Currency> currencies);

