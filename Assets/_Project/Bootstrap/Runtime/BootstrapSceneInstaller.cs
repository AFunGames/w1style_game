using Zenject;

namespace W1Style.Bootstrap
{
    /// <summary>
    /// Scene-level installer for the Bootstrap scene.
    /// Binds Bootstrap-specific components to the SceneContext.
    /// </summary>
    public sealed class BootstrapSceneInstaller : MonoInstaller<BootstrapSceneInstaller>
    {
        public override void InstallBindings()
        {
            // BootstrapController is a MonoBehaviour in the scene;
            // it receives injection automatically via SceneContext.
            // No additional scene-level bindings needed at baseline.
        }
    }
}
