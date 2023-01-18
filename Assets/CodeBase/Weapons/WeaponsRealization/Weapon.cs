using System;
using System.Collections;
using CodeBase.Infrastructure.Factory;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Zenject;
using Random = UnityEngine.Random;

namespace CodeBase.Weapons
{
    public class Weapon : PlayerWeapon
    {
        [SerializeField] private HitScan _hitScan;
        [SerializeField] private bool _addBulletSpread;
        [SerializeField] private Vector3 _bulletSpreadVariance = new Vector3(0.1f, 0.1f, 0.1f);
        [SerializeField] private ParticleSystem _shootingSystem;
        [SerializeField] private Transform _shootOrigin;
        [SerializeField] private TrailRenderer _bulletTrail;
        [SerializeField] private PhotonView _photonView;
        private IGameFactory _gameFactory;
        private int _currentAmmo;
        private float _lastShootTime;
        private bool _cantShoot;
        public override event Action<float> OnReloadPercent;

        public override void Construct(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        private void Start()
        {
            _currentAmmo = MagazineCount;
        }


        public override void Shoot()
        {
            int trailId = PhotonNetwork.AllocateViewID(false);
            //  _photonView.RPC("ShootNetwork", RpcTarget.All, trailId);
            ShootNetwork();
        }

        private void ShootNetwork()
        {
            if (!_photonView)
                return;


            if (_cantShoot || _currentAmmo <= 0)
                return;

            if (_lastShootTime + ShootDelay < Time.time)
            {
                RaycastHit hitInfo = _hitScan.GetHit(_shootOrigin.forward);

                GameObject trailGO = _gameFactory.CreateGameObject("Trail", _shootOrigin.transform.position,
                    Quaternion.identity);
                TrailRenderer trail = trailGO.GetComponent<TrailRenderer>();
                
                StartCoroutine(SpawnTrail(trail, hitInfo));
                
                PhotonView enemyPhotonView = hitInfo.collider?.GetComponent<PhotonView>();
                if (enemyPhotonView)
                    enemyPhotonView.RPC("TakeDamage", RpcTarget.AllBuffered, Damage);
                _shootingSystem.Play();

                _currentAmmo--;

                if (_currentAmmo <= 0)
                {
                    StartCoroutine(Reload());
                }

                _lastShootTime = Time.time;
            }
        }

        private Vector3 GetDirection()
        {
            Vector3 direction = transform.forward;
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

        private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hitInfo)
        {
            float time = 0;
            Vector3 startPos = trail.transform.position;

            while (time < 1)
            {
                trail.transform.position = Vector3.Lerp(startPos, hitInfo.point, time);
                time += Time.deltaTime / trail.time;
                yield return null;
            }

            trail.transform.position = hitInfo.point;

            Destroy(trail.gameObject, trail.time);
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
            OnReloadPercent?.Invoke(0);
        }
    }
}