using System.Collections.Generic;
using System.Linq;
using W1Style.Core.Interfaces;
using W1Style.UI.Core;

namespace W1Style.UI.Services
{
    /// <summary>
    /// Concrete UI service managing registered UI panels.
    /// Panels register themselves or are collected at scene start.
    /// Bound per scene via SceneContext installer.
    /// </summary>
    public sealed class UIService : IUIService
    {
        private readonly List<UIPanel> _panels;
        private readonly ILogService _log;

        public UIService(List<UIPanel> panels, ILogService log)
        {
            _panels = panels;
            _log = log;
        }

        public void ShowPanel<T>() where T : UIPanel
        {
            var panel = GetPanel<T>();
            if (panel == null)
            {
                _log.Warning($"UIPanel of type {typeof(T).Name} not found.");
                return;
            }

            panel.Show();
        }

        public void HidePanel<T>() where T : UIPanel
        {
            var panel = GetPanel<T>();
            if (panel == null)
            {
                _log.Warning($"UIPanel of type {typeof(T).Name} not found.");
                return;
            }

            panel.Hide();
        }

        public void HideAll()
        {
            foreach (var panel in _panels)
            {
                panel.Hide();
            }
        }

        public T GetPanel<T>() where T : UIPanel
        {
            return _panels.OfType<T>().FirstOrDefault();
        }

        public bool IsPanelVisible<T>() where T : UIPanel
        {
            var panel = GetPanel<T>();
            return panel != null && panel.IsVisible;
        }
    }
}
