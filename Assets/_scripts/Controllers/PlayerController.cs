using UnityEngine;
using DG.Tweening;

namespace BallObstacleGame
{
    public class PlayerController : Singleton<PlayerController>
    {
        [SerializeField] private Transform _visual;
        [SerializeField] private Transform _sizeVisual;
        [SerializeField, Header("Main Settings")] private float _defaultPlayerMass = 10f;
        [SerializeField] private float _massLossingModifire = 1f;
        [SerializeField, Header("Fire Setting")] private Transform _firePoint;
        [SerializeField] private float _bulletSpawnAnimDuration = 1f;
        [SerializeField] private float _maxChangeDuration = 5f;
        [SerializeField] private float _lossingMassPerFire = 0.2f;
        [SerializeField, Header("Default Bullet Settings")] private float _defaultBulletSpeed = 10f;
        [SerializeField] private float _defaultExplosionRadius = 2f;
        [SerializeField] private float _defaultScale = 2f;
        [SerializeField, Header("End Bullet Settings")] private float _bulletEndSpeed = 2f;
        [SerializeField] private float _bulletEndExplosionRadius = 8f;
        private const string _bulletTag = "Bullet";
        private float _bulletFinalSpeed = 0f;
        private float _bulletFinalRadius = 0f;
        private float _playerMass = 0f;
        private float _savedPlayerMass = 0f;
        private float _elapsedTime = 0f;
        private bool _spawned;
        private bool _grow;
        private bool _canInput;
        private Bullet _bullet;
        private Sequence _spawnAnimSequence;
        private ObjectPooling _objectPooling;
        private IInput _input;

        public bool CanInput
        {
            get => _canInput;
            set => _canInput = value;
        }

        private void Start()
        {
            Initialize();
        }

        private void Update()
        {
            if (!CanInput) return;

            if (_input.OnTouch() && !_spawned)
            {
                SpawnBullet();
                ResetChangeSizeSettings();
            }

            if (_input.OnHold() && _grow)
            {
                ChangeSize();
            }

            if (_input.OnRelease() && _spawned)
            {
                Shoot();
            }
        }

        private void Initialize()
        {
            _objectPooling = ObjectPooling.Instance;
            _input = new MobileDesktopInput();
            _playerMass = _defaultPlayerMass;
            CalculateRoadSize();
        }

        private void SpawnBullet()
        {
            _playerMass -= _lossingMassPerFire;
            if (!DidPlayerMassBiggerThanZero()) return;

            _spawned = true;

            GameObject bullet = _objectPooling.SpawnFromPool(_bulletTag, _visual.position, Quaternion.identity);
            Transform bulletT = bullet.transform;
            _bullet = bullet.GetComponent<Bullet>();
            _bullet.DisableMove();

            _spawnAnimSequence = DOTween.Sequence();
            _spawnAnimSequence.Join(bulletT.DOMove(_firePoint.position, _bulletSpawnAnimDuration));
            _spawnAnimSequence.Play();
        }

        private void ResetChangeSizeSettings()
        {
            _elapsedTime = 0f;
            _savedPlayerMass = _playerMass;
            _bulletFinalSpeed = _defaultBulletSpeed;
            _bulletFinalRadius = _defaultExplosionRadius;
            _grow = true;
        }

        private void ChangeSize()
        {
            float modifireValue = _massLossingModifire * Time.deltaTime;
            float playerLerp = 1f - _playerMass / _defaultPlayerMass;
            float bulletLerp = _elapsedTime / _maxChangeDuration;
            Vector3 newPlayerScale = Vector3.Slerp(Vector3.one * _defaultPlayerMass, Vector3.one * _playerMass, playerLerp);
            Vector3 newBulletScale = Vector3.Slerp(Vector3.one * _defaultScale, Vector3.one * _savedPlayerMass, bulletLerp);
            _bulletFinalSpeed = Mathf.Lerp(_defaultBulletSpeed, _bulletEndSpeed, bulletLerp);
            _bulletFinalRadius = Mathf.Lerp(_defaultExplosionRadius, _bulletEndExplosionRadius, bulletLerp);

            _playerMass -= modifireValue;
            _elapsedTime += Time.deltaTime;

            _visual.localScale = newPlayerScale;
            _bullet.transform.localScale = newBulletScale;
            CalculateRoadSize();

            if (!DidPlayerMassBiggerThanZero())
            {
                Shoot();
                return;
            }

            if (_elapsedTime >= _maxChangeDuration)
            {
                Shoot();
                return;
            }
        }

        private void Shoot()
        {
            ClearRoadObserver.Instance.Observe();
            _spawnAnimSequence.Kill();

            if (_bullet != null)
            {
                _bullet.Init(new BulletStats(_bulletFinalSpeed, _bulletFinalRadius));
                _bullet.EnableMove();
            }

            _spawned = false;
            _grow = false;
        }

        private void CalculateRoadSize()
        {
            _sizeVisual.localScale = new Vector3(_visual.localScale.x, _sizeVisual.localScale.y, _sizeVisual.localScale.z);
        }

        private bool DidPlayerMassBiggerThanZero()
        {
            bool result = _playerMass <= 0 ? false : true;

            if (!result)
            {
                _playerMass = 0;
                CanInput = false;
                GameManager.Instance.Lose();
            }

            return result;
        }
    }
}