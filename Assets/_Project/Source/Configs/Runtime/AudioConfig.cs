using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace W1Style.Configs
{
    /// <summary>
    /// Audio configuration values.
    /// Holds volume defaults and audio-related settings.
    /// </summary>
    [CreateAssetMenu(fileName = "AudioConfig", menuName = "W1Style/Configs/Audio Config")]
    public sealed class AudioConfig : ScriptableObject
    {
#if ODIN_INSPECTOR
        [Title("Volume Settings")]
        [PropertyRange(0f, 1f)]
#endif
        [Range(0f, 1f)]
        [SerializeField] private float _masterVolume = 1f;

#if ODIN_INSPECTOR
        [PropertyRange(0f, 1f)]
#endif
        [Range(0f, 1f)]
        [SerializeField] private float _musicVolume = 0.8f;

#if ODIN_INSPECTOR
        [PropertyRange(0f, 1f)]
#endif
        [Range(0f, 1f)]
        [SerializeField] private float _sfxVolume = 1f;

        public float MasterVolume => _masterVolume;
        public float MusicVolume => _musicVolume;
        public float SfxVolume => _sfxVolume;
    }
}
