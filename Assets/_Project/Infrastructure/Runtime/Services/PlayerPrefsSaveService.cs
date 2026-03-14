using UnityEngine;
using W1Style.Core.Interfaces;

namespace W1Style.Infrastructure.Services
{
    /// <summary>
    /// JSON-based save service using Unity's PlayerPrefs as storage backend.
    /// Suitable for lightweight save data. Replace with file-based or cloud
    /// storage for larger datasets.
    /// Bound as a global singleton via ProjectInstaller.
    /// </summary>
    public sealed class PlayerPrefsSaveService : ISaveService
    {
        private readonly ILogService _log;

        public PlayerPrefsSaveService(ILogService log)
        {
            _log = log;
        }

        public void Save<T>(string key, T data)
        {
            var json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
            _log.Info($"Saved data for key: {key}");
        }

        public T Load<T>(string key, T defaultValue = default)
        {
            if (!PlayerPrefs.HasKey(key))
                return defaultValue;

            var json = PlayerPrefs.GetString(key);
            return JsonUtility.FromJson<T>(json);
        }

        public bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }

        public void Delete(string key)
        {
            PlayerPrefs.DeleteKey(key);
            _log.Info($"Deleted save data for key: {key}");
        }

        public void DeleteAll()
        {
            PlayerPrefs.DeleteAll();
            _log.Info("Deleted all save data.");
        }
    }
}
