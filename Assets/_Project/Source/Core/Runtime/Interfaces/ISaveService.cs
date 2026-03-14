namespace W1Style.Core.Interfaces
{
    /// <summary>
    /// Abstraction for save/load operations.
    /// Implementation can be JSON-file, binary, PlayerPrefs, or cloud-based.
    /// </summary>
    public interface ISaveService
    {
        void Save<T>(string key, T data);
        T Load<T>(string key, T defaultValue = default);
        bool HasKey(string key);
        void Delete(string key);
        void DeleteAll();
    }
}
