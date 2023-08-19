using UnityEngine;
using DG.Tweening;

namespace BallObstacleGame
{
    public class BulletStats
    {
        public float Speed;
        public float ExplosionRadius;

        public BulletStats(float speed, float explosionRadius)
        {
            Speed = speed;
            ExplosionRadius = explosionRadius;
        }
    }

    public class Bullet : MonoBehaviour
    {
        private const string _explosionEffectTag = "ExplosionEffect";
        private BulletStats _bulletStats;
        private bool _move = false;

        private void Update()
        {
            if(_move) Move();
        }

        public void Init(BulletStats bulletStats)
        {
            _bulletStats = bulletStats;
        }

        private void Move()
        {
            transform.Translate(transform.forward * _bulletStats.Speed * Time.deltaTime);
        }

        private async void Explosion()
        {
            DisableMove();

            GameObject explosionEffect = ObjectPooling.Instance.SpawnFromPool(_explosionEffectTag, transform.position, Quaternion.identity);
            var explosionSequence = DOTween.Sequence();
            float sphereSize = _bulletStats.ExplosionRadius * 2f;
            float animationDuration = 0.5f;

            explosionSequence
                .Append(explosionEffect.transform.DOScale(Vector3.one * sphereSize, animationDuration)).SetEase(Ease.InCubic)
                .AppendCallback(() =>
                {
                    Collider[] hitColliders = Physics.OverlapSphere(transform.position, _bulletStats.ExplosionRadius);
                    foreach (var hitCollider in hitColliders)
                        if (hitCollider.TryGetComponent(out Obstacle obstacle)) obstacle.Infect();

                    gameObject.SetActive(false);
                })
                .Append(explosionEffect.transform.DOScale(Vector3.zero, animationDuration)).SetEase(Ease.OutCubic)
                .OnComplete(()=> explosionEffect.SetActive(false));
            await explosionSequence.Play().AsyncWaitForCompletion();
        }

        public void DisableMove()
        {
            _move = false;
        }

        public void EnableMove()
        {
            _move = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            Explosion();
        }
    }
}