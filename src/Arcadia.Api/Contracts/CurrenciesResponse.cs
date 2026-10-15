namespace Arcadia.Api.Contracts;

public record Currency(long CurrencyId, int Amount);

public record CurrenciesResponse(List<Currency> Currencies);

