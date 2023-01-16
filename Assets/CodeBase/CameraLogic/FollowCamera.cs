using System;
using CodeBase.Services.Update;
using Photon.Pun;
using UnityEngine;
using Zenject;

namespace CodeBase.CameraLogic
{
    public class FollowCamera : MonoBehaviour, ILateUpdatable
    {
        [SerializeField]private Vector3 _offset;
        [SerializeField]private float _smoothSpeed= 0.3f;
        private GameObject _player;
        private IUpdateService _updateService;
        private PhotonView _photonView;

        private void Start()
        {
            _photonView = GetComponent<PhotonView>();
        
        }

        [Inject]
        public void Construct(IUpdateService updateService)
        {
            _updateService = updateService;
            updateService.Register(this);
        }

        public void Setup(GameObject player)
        {
            _player = player;
        }

        private void OnDestroy()
        {
            _updateService.Unregister(this);
        }

        public void LateUpdateTick()
        {
            if (!_player || !_photonView.IsMine)
                return;
         
            Vector3 targetPosition = _player.transform.position + _offset;
            Vector3 smoothFollow = Vector3.Lerp(transform.position,targetPosition,_smoothSpeed);
            transform.position = new Vector3(smoothFollow.x,transform.position.y,smoothFollow.z);
        }
    }
}