using System;
using System.Collections;
using CodeBase.Infrastructure.Factory;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CodeBase.Weapons.WeaponsRealization
{
    public class Weapon : PlayerWeapon
    {
        [SerializeField] private HitScan _hitScan;
        [SerializeField] private bool _addBulletSpread;
        [SerializeField] private Vector3 _bulletSpreadVariance = new Vector3(0.1f, 0.1f, 0.1f);
        [SerializeField] private ParticleSystem _shootingSystem;
        [SerializeField] private Transform _shootOrigin;
        [SerializeField] private PhotonView _photonView;
        private IGameFactory _gameFactory;
        private int _currentAmmo;
        private float _lastShootTime;
        private bool _cantShoot;
        public override event Action<float> OnReloadPercent;
        public override event Action<int> OnAmmoCount;

        public override void Construct(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        private void Start()
        {
            _currentAmmo = MagazineCount;
            _shootingSystem.Stop();
            OnAmmoCount?.Invoke(_currentAmmo);
        }


        public override void Shoot()
        {
            //  _photonView.RPC("ShootNetwork", RpcTarget.All, trailId);
            if (!_photonView)
                return;


            if (_cantShoot || _currentAmmo <= 0)
                return;

            if (_lastShootTime + ShootDelay < Time.time)
            {
                RaycastHit hitInfo = _hitScan.GetHit(GetDirection(_shootOrigin));

                GameObject trailGO = _gameFactory.CreateBullet(BulletType, _shootOrigin.transform.position,
                    _shootOrigin.rotation);
                TrailRenderer trail = trailGO.GetComponentInChildren<TrailRenderer>();
                
                StartCoroutine(SpawnTrail(trailGO,trail, hitInfo));
                
                PhotonView enemyPhotonView = hitInfo.collider?.GetComponent<PhotonView>();
                if (enemyPhotonView)
                    enemyPhotonView.RPC("TakeDamage", RpcTarget.AllBuffered, Damage);
                _shootingSystem.Play();

                _currentAmmo--;
                OnAmmoCount?.Invoke(_currentAmmo);
                if (_currentAmmo <= 0)
                {
                    StartCoroutine(Reload());
                }

                _lastShootTime = Time.time;
            }
        }

        private Vector3 GetDirection(Transform shootOrigin)
        {
            Vector3 direction = shootOrigin.forward;
            if (_addBulletSpread)
            {
                direction += new Vector3(
                    Random.Range(-_bulletSpreadVariance.x, _bulletSpreadVariance.x),
                    Random.Range(-_bulletSpreadVariance.y, _bulletSpreadVariance.y),
                    Random.Range(-_bulletSpreadVariance.z, _bulletSpreadVariance.z)
                );
                direction.Normalize();
            }

            return direction;
        }

        private IEnumerator SpawnTrail(GameObject bullet,TrailRenderer trail, RaycastHit hitInfo)
        {
            float time = 0;
            Vector3 startPos = bullet.transform.position;

            while (time < 1)
            {
                bullet.transform.position = Vector3.Lerp(startPos, hitInfo.point, time);
                time += Time.deltaTime / trail.time;
                yield return null;
            }
            bullet.transform.position = hitInfo.point;
            
            Destroy(bullet.gameObject, trail.time);
        }

        private IEnumerator Reload()
        {
            _cantShoot = true;
            OnReloadPercent?.Invoke(0);
            float _time = 0;
            while (_time < ReloadDelay)
            {
                _time += Time.deltaTime;
                OnReloadPercent?.Invoke(_time / ReloadDelay);
                yield return null;
            }

            _cantShoot = false;
            _currentAmmo = MagazineCount;
            OnAmmoCount?.Invoke(_currentAmmo);
            OnReloadPercent?.Invoke(0);
        }
    }
}