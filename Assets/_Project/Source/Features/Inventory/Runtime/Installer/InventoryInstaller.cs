using W1Style.Features.Inventory.Config;
using W1Style.Features.Inventory.Domain;
using W1Style.Features.Inventory.Services;
using Zenject;

namespace W1Style.Features.Inventory.Installer
{
    /// <summary>
    /// Zenject installer for the Inventory feature.
    /// Can be added to a SceneContext for scene-level inventory,
    /// or to ProjectContext for a global persistent inventory.
    ///
    /// Requires InventoryConfig to be assigned in the inspector.
    /// </summary>
    public sealed class InventoryInstaller : MonoInstaller<InventoryInstaller>
    {
        [UnityEngine.SerializeField] private InventoryConfig _config;

        public override void InstallBindings()
        {
            Container.Bind<InventoryConfig>()
                .FromInstance(_config)
                .AsSingle();

            Container.Bind<IInventoryService>()
                .To<InventoryService>()
                .AsSingle();
        }
    }
}
