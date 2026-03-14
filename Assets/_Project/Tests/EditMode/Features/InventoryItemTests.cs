using NUnit.Framework;
using W1Style.Features.Inventory.Domain;

namespace W1Style.Tests.EditMode.Features
{
    /// <summary>
    /// Unit tests for InventoryItem domain model.
    /// Pure C# tests — no Unity scene required.
    /// </summary>
    [TestFixture]
    public sealed class InventoryItemTests
    {
        [Test]
        public void Constructor_SetsPropertiesCorrectly()
        {
            var item = new InventoryItem("sword_01", "Iron Sword", 1, 1);

            Assert.AreEqual("sword_01", item.Id);
            Assert.AreEqual("Iron Sword", item.DisplayName);
            Assert.AreEqual(1, item.Quantity);
            Assert.AreEqual(1, item.MaxStack);
        }

        [Test]
        public void Constructor_DefaultValues_AreApplied()
        {
            var item = new InventoryItem("potion_01", "Health Potion");

            Assert.AreEqual(1, item.Quantity);
            Assert.AreEqual(99, item.MaxStack);
        }

        [Test]
        public void CanStack_WhenBelowMax_ReturnsTrue()
        {
            var item = new InventoryItem("arrow_01", "Arrow", 10, 64);

            Assert.IsTrue(item.CanStack(50));
        }

        [Test]
        public void CanStack_WhenAtMax_ReturnsFalse()
        {
            var item = new InventoryItem("arrow_01", "Arrow", 60, 64);

            Assert.IsFalse(item.CanStack(5));
        }

        [Test]
        public void CanStack_WhenExactlyAtMax_ReturnsTrue()
        {
            var item = new InventoryItem("arrow_01", "Arrow", 60, 64);

            Assert.IsTrue(item.CanStack(4));
        }
    }
}
