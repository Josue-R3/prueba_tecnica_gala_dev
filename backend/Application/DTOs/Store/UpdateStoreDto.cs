using Domain.Enums;

namespace Application.DTOs.Store;

public record UpdateStoreDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Address { get; init; } = string.Empty;
    public StoreStatus Status { get; init; }
}
