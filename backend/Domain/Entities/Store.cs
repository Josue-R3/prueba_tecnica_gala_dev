using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class Store : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public StoreStatus Status { get; set; }
}
