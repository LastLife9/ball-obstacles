using UnityEngine;

namespace BallObstacleGame
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private VirtualPanel[] _gamePanels;

        public void EnablePanel(PanelType type)
        {
            foreach (var panel in _gamePanels)
                panel.gameObject.SetActive(panel.Type.Equals(type));
        }
    }
}