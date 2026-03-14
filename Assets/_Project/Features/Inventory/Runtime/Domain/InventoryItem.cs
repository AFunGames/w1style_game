namespace W1Style.Features.Inventory.Domain
{
    /// <summary>
    /// Plain data model for an inventory item.
    /// Pure C# — no Unity dependencies.
    /// </summary>
    [System.Serializable]
    public sealed class InventoryItem
    {
        public string Id;
        public string DisplayName;
        public int Quantity;
        public int MaxStack;

        public InventoryItem(string id, string displayName, int quantity = 1, int maxStack = 99)
        {
            Id = id;
            DisplayName = displayName;
            Quantity = quantity;
            MaxStack = maxStack;
        }

        public bool CanStack(int amount)
        {
            return Quantity + amount <= MaxStack;
        }
    }
}
