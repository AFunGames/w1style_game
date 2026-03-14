using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace W1Style.Features.Inventory.Config
{
    /// <summary>
    /// Configuration for the Inventory feature.
    /// Defines capacity limits and default stacking behavior.
    /// </summary>
    [CreateAssetMenu(fileName = "InventoryConfig", menuName = "W1Style/Features/Inventory Config")]
    public sealed class InventoryConfig : ScriptableObject
    {
#if ODIN_INSPECTOR
        [Title("Inventory Settings")]
        [MinValue(1)]
#endif
        [Min(1)]
        [SerializeField] private int _maxInventorySlots = 20;

#if ODIN_INSPECTOR
        [MinValue(1)]
#endif
        [Min(1)]
        [SerializeField] private int _defaultMaxStack = 99;

        public int MaxInventorySlots => _maxInventorySlots;
        public int DefaultMaxStack => _defaultMaxStack;
    }
}
