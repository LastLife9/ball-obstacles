using UnityEngine;
using UnityEngine.EventSystems;

namespace BallObstacleGame
{
    public class TapToPlayScreen : MonoBehaviour, IPointerDownHandler
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            GameManager.Instance.StartGame();
        }
    }
}