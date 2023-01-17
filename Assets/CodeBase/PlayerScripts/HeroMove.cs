using CodeBase.Services.Input;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.Update;
using Photon.Pun;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace CodeBase.PlayerScripts
{
    public class HeroMove : MonoBehaviour, IUpdatable
    {
        [Required]
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private float _movementSpeed = 1;
        
        private IInputService _inputService;
        private IUpdateService _updateService;
        private ISaveLoadService _saveLoadService;
        private PhotonView photonView;
        private Camera _camera;
        private Vector3 _startPos;
        
        [Inject]
        public void Construct(IInputService inputService,
            ISaveLoadService saveLoadService, IUpdateService updateService)
        {
            _inputService = inputService;
            _saveLoadService = saveLoadService;
            _updateService = updateService;
            _updateService.Register(this);
            _startPos = transform.position;
        }

        private void OnDisable()
        {
            _updateService.Unregister(this);
            _saveLoadService.SaveProgress();
        }

        private void Start()
        {
            photonView = GetComponent<PhotonView>();
        }

        public void Setup(Camera camera)
        {
            _camera = camera;
        }

        public void UpdateTick()
        {
            if(!_camera)
                return;
            if (!photonView || !photonView.IsMine)
                return;
        
            Vector3 movementAxis = Vector3.zero;
            Vector3 movementVector = Vector3.zero;
            Vector2 positionOnScreen = _camera.WorldToViewportPoint(transform.position);
            Vector2 mouseOnScreen = _camera.ScreenToViewportPoint(Input.mousePosition);
            float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);
            if (_inputService.Axis.sqrMagnitude > Constants.Epsilon)
            {
                movementAxis = _inputService.Axis;
                movementAxis.z = movementAxis.y;
                movementAxis.y = 0;
                movementVector += transform.forward * movementAxis.z;
                movementVector += transform.right * movementAxis.x;
                movementVector.Normalize();
            }
            transform.rotation =  Quaternion.Euler (new Vector3(0f,-angle - 90,0f));
            _characterController.Move(_movementSpeed * movementVector * Time.deltaTime);
        }
        
        float AngleBetweenTwoPoints(Vector3 a, Vector3 b) {
            return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
        }

        public void ResetPosition()
        {
            transform.position = _startPos;
        }
    }
}