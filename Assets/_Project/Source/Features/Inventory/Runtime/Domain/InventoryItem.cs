namespace W1Style.Features.Inventory.Domain
{
    /// <summary>
    /// Plain data model for an inventory item.
    /// Pure C# — no Unity dependencies.
    /// </summary>
    [System.Serializable]
    public sealed class InventoryItem
    {
        public string Id { get; private set; }
        public string DisplayName { get; private set; }
        public int Quantity { get; private set; }
        public int MaxStack { get; private set; }

        public InventoryItem(string id, string displayName, int quantity = 1, int maxStack = 99)
        {
            Id = id;
            DisplayName = displayName;
            Quantity = System.Math.Max(0, quantity);
            MaxStack = System.Math.Max(1, maxStack);
        }

        public bool CanStack(int amount)
        {
            return Quantity + amount <= MaxStack;
        }

        /// <summary>
        /// Increases quantity by the given amount, clamped to MaxStack.
        /// Returns true if the full amount was added, false if clamped.
        /// </summary>
        public bool AddQuantity(int amount)
        {
            if (amount <= 0) return false;
            var newQuantity = Quantity + amount;
            if (newQuantity > MaxStack)
            {
                Quantity = MaxStack;
                return false;
            }

            Quantity = newQuantity;
            return true;
        }

        /// <summary>
        /// Decreases quantity by the given amount. Quantity will not go below zero.
        /// Returns the actual amount removed.
        /// </summary>
        public int RemoveQuantity(int amount)
        {
            if (amount <= 0) return 0;
            var removed = System.Math.Min(amount, Quantity);
            Quantity -= removed;
            return removed;
        }

        public bool IsEmpty => Quantity <= 0;
    }
}
