using UnityEngine;

namespace BallObstacleGame
{
    public class VirtualPanel : VirtualUIElement
    {
        [SerializeField] private PanelType _type;

        public PanelType Type => _type;
    }
}