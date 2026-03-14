using System.Collections.Generic;
using System.Linq;
using W1Style.Core.Interfaces;
using W1Style.Features.Inventory.Config;
using W1Style.Features.Inventory.Domain;

namespace W1Style.Features.Inventory.Services
{
    /// <summary>
    /// Concrete inventory service implementation.
    /// Manages a list of inventory items with add/remove/query operations.
    /// Config-driven via InventoryConfig for max capacity.
    /// </summary>
    public sealed class InventoryService : IInventoryService
    {
        private readonly List<InventoryItem> _items = new();
        private readonly InventoryConfig _config;
        private readonly ILogService _log;

        public IReadOnlyList<InventoryItem> Items => _items;

        public InventoryService(InventoryConfig config, ILogService log)
        {
            _config = config;
            _log = log;
        }

        public bool AddItem(string itemId, string displayName, int quantity = 1)
        {
            var existing = _items.FirstOrDefault(i => i.Id == itemId);

            if (existing != null)
            {
                if (!existing.CanStack(quantity))
                {
                    _log.Warning($"Cannot stack {quantity} more of '{itemId}'. Max stack reached.");
                    return false;
                }

                existing.Quantity += quantity;
                _log.Info($"Stacked {quantity}x '{itemId}'. Total: {existing.Quantity}");
                return true;
            }

            if (_items.Count >= _config.MaxInventorySlots)
            {
                _log.Warning($"Inventory full. Cannot add '{itemId}'.");
                return false;
            }

            _items.Add(new InventoryItem(itemId, displayName, quantity, _config.DefaultMaxStack));
            _log.Info($"Added {quantity}x '{itemId}' to inventory.");
            return true;
        }

        public bool RemoveItem(string itemId, int quantity = 1)
        {
            var existing = _items.FirstOrDefault(i => i.Id == itemId);
            if (existing == null)
                return false;

            existing.Quantity -= quantity;
            if (existing.Quantity <= 0)
            {
                _items.Remove(existing);
                _log.Info($"Removed '{itemId}' from inventory.");
            }
            else
            {
                _log.Info($"Removed {quantity}x '{itemId}'. Remaining: {existing.Quantity}");
            }

            return true;
        }

        public InventoryItem GetItem(string itemId)
        {
            return _items.FirstOrDefault(i => i.Id == itemId);
        }

        public bool HasItem(string itemId, int quantity = 1)
        {
            var item = _items.FirstOrDefault(i => i.Id == itemId);
            return item != null && item.Quantity >= quantity;
        }

        public void Clear()
        {
            _items.Clear();
            _log.Info("Inventory cleared.");
        }
    }
}
