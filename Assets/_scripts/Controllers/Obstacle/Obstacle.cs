using UnityEngine;

namespace BallObstacleGame
{
    public enum ObstacleState
    {
        Base,
        Infected,
        Destroy
    }

    public class Obstacle : MonoBehaviour
    {
        [SerializeField] private ObstacleVisual[] _visuals;
        [SerializeField] private float _timeToDestroy = 2f;

        private void Start()
        {
            ChangeState(ObstacleState.Base);
        }

        private void ChangeState(ObstacleState state)
        {
            foreach (var visual in _visuals)
                visual.gameObject.SetActive(visual.State.Equals(state));
        }

        public void Infect()
        {
            ChangeState(ObstacleState.Infected);
            Invoke(nameof(DestroyObstacle), _timeToDestroy);
        }

        public void DestroyObstacle()
        {
            ChangeState(ObstacleState.Destroy);

            Collider coll = GetComponent<Collider>();
            coll.enabled = false;
            ClearRoadObserver.Instance.Observe();
        }
    }
}
