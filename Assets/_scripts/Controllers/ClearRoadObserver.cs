using UnityEngine;

namespace BallObstacleGame
{
    public class ClearRoadObserver : Singleton<ClearRoadObserver>
    {
        [SerializeField] private Transform[] _raycastPoints;
        int _obstacleLayer = 1 << 8;

        public void Observe()
        {
            if (!CheckForObstacles())
            {
                PlayerMove.Instance.StartMove();
                PlayerController.Instance.CanInput = false;
            }
        }

        private bool CheckForObstacles()
        {
            float dist = PlayerMove.Instance.DistanceToExit;

            for (int i = 0; i < _raycastPoints.Length; i++)
            {
                Vector3 startPos = _raycastPoints[i].position;
                Vector3 exitPos = startPos + (Vector3.forward * dist);
                Ray ray = new Ray(_raycastPoints[i].position, exitPos - startPos);

                if (Physics.Raycast(ray, dist, _obstacleLayer))
                {
                    return true;
                }
            }
            
            return false;
        }
    }
}
