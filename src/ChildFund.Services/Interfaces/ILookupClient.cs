using ChildFund.Services.Models;

namespace ChildFund.Services.Interfaces;

public interface ILookupClient
{
    Task<CountryDto[]> GetAllCountriesAsync(CancellationToken ct = default);
}

