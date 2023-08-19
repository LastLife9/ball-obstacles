using UnityEngine;

namespace BallObstacleGame
{
    public class ObstacleVisual : MonoBehaviour
    {
        [SerializeField] private ObstacleState _state;

        public ObstacleState State => _state;
    }
}