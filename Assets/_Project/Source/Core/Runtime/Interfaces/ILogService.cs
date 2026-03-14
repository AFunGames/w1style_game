namespace W1Style.Core.Interfaces
{
    /// <summary>
    /// Abstraction for logging, allowing replacement of Unity's Debug.Log
    /// with structured or filtered logging in production builds.
    /// </summary>
    public interface ILogService
    {
        void Info(string message);
        void Warning(string message);
        void Error(string message);
    }
}
