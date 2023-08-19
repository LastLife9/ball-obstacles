using DG.Tweening;
using UnityEngine;

namespace BallObstacleGame
{
    public class PlayerMove : Singleton<PlayerMove>
    {
        [SerializeField] private Transform _exit;
        [SerializeField] private GameObject[] _objectsToDisable;
        [SerializeField] private float _bounceHeight = 2f;
        [SerializeField] private float _speed = 0.5f;

        public float DistanceToExit => Vector3.Distance(transform.position, _exit.position);

        public async void StartMove()
        {
            foreach (var item in _objectsToDisable)
            {
                item.SetActive(false);
            }

            var moveSequence = DOTween.Sequence();
            int bounceCount = (int)DistanceToExit / 4;
            float bounceDuration = _speed * 0.5f;

            for (int i = 0; i < bounceCount; i++)
            {
                float normalizedBounceHeight = Mathf.Lerp(0, _bounceHeight, Mathf.Sin((i / (float)bounceCount) * Mathf.PI));
                float targetZ = Mathf.Lerp(transform.position.z, _exit.position.z, (i + 1) / (float)bounceCount);

                moveSequence
                    .Append(transform.DOMoveY(normalizedBounceHeight, bounceDuration).SetEase(Ease.OutQuad))
                    .Append(transform.DOMoveY(0, bounceDuration).SetEase(Ease.InQuad))
                    .Join(transform.DOMoveZ(targetZ, bounceDuration).SetEase(Ease.Linear));
            }

            await moveSequence.Play().AsyncWaitForCompletion();

            GameManager.Instance.Win();
        }
    }
}