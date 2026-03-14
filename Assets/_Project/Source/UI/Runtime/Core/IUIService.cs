namespace W1Style.UI.Core
{
    /// <summary>
    /// Interface for the UI service that manages panel visibility.
    /// Keeps UI management centralized and testable.
    /// </summary>
    public interface IUIService
    {
        void ShowPanel<T>() where T : UIPanel;
        void HidePanel<T>() where T : UIPanel;
        void HideAll();
        T GetPanel<T>() where T : UIPanel;
        bool IsPanelVisible<T>() where T : UIPanel;
    }
}
