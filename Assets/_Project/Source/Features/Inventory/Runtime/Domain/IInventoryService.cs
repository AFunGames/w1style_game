using System.Collections.Generic;

namespace W1Style.Features.Inventory.Domain
{
    /// <summary>
    /// Interface for the inventory system.
    /// Defined in the Domain layer so it can be referenced
    /// without depending on the concrete implementation.
    /// </summary>
    public interface IInventoryService
    {
        IReadOnlyList<InventoryItem> Items { get; }
        bool AddItem(string itemId, string displayName, int quantity = 1);
        bool RemoveItem(string itemId, int quantity = 1);
        InventoryItem GetItem(string itemId);
        bool HasItem(string itemId, int quantity = 1);
        void Clear();
    }
}
