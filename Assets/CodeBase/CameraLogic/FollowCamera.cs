using System;
using CodeBase.Services.Update;
using Photon.Pun;
using UnityEngine;
using Zenject;

namespace CodeBase.CameraLogic
{
    public class FollowCamera : MonoBehaviour, ILateUpdatable
    {
        [SerializeField] private Vector3 _offset;
        [SerializeField] private float _smoothSpeed = 0.3f;
        private GameObject _player;
        private IUpdateService _updateService;

        [Inject]
        public void Construct(IUpdateService updateService)
        {
            _updateService = updateService;
            updateService.Register(this);
        }

        public void Setup(GameObject player)
        {
            _player = player;
            Vector3 direction = _player.transform.position - Vector3.zero;
            direction.Normalize();
            float dotDirection = Vector3.Dot(transform.up, direction);

            if (dotDirection > 0.9f)
            {
                transform.rotation = Quaternion.Euler(new Vector3(transform.eulerAngles.x,
                    -180, transform.eulerAngles.z));
            }
        }

        private void OnDestroy()
        {
            if (_updateService != null)
                _updateService.Unregister(this);
        }

        public void LateUpdateTick()
        {
            if (!_player)
                return;

            Vector3 targetPosition = _player.transform.position + _offset;
            Vector3 smoothFollow = Vector3.Lerp(transform.position, targetPosition, _smoothSpeed);
            transform.position = new Vector3(smoothFollow.x, transform.position.y, smoothFollow.z);
        }

        float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
        {
            return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
        }
    }
}