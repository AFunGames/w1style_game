using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace W1Style.Configs
{
    /// <summary>
    /// UI configuration values.
    /// Holds default UI behavior settings.
    /// </summary>
    [CreateAssetMenu(fileName = "UIConfig", menuName = "W1Style/Configs/UI Config")]
    public sealed class UIConfig : ScriptableObject
    {
#if ODIN_INSPECTOR
        [Title("Animation")]
#endif
        [Header("Transition")]
        [SerializeField] private float _panelFadeDuration = 0.3f;
        [SerializeField] private float _screenTransitionDuration = 0.5f;

#if ODIN_INSPECTOR
        [Title("Layout")]
#endif
        [Header("Layout")]
        [SerializeField] private bool _useCanvasScaler = true;
        [SerializeField] private float _referenceWidth = 1920f;
        [SerializeField] private float _referenceHeight = 1080f;

        public float PanelFadeDuration => _panelFadeDuration;
        public float ScreenTransitionDuration => _screenTransitionDuration;
        public bool UseCanvasScaler => _useCanvasScaler;
        public float ReferenceWidth => _referenceWidth;
        public float ReferenceHeight => _referenceHeight;
    }
}
