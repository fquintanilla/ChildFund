using ChildFund.Services.Models;

namespace ChildFund.Services.Interfaces;

public interface IChildInventoryClient
{
    Task<ChildSummaryDto[]> GetRandomKidsForWebAsync(CancellationToken ct = default);

    Task<AvailableKidsForWebResponseDto> GetAvailableKidsForWebAsync(int countryCode, CancellationToken ct = default);
}

