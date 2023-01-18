using CodeBase.Services.Input;
using CodeBase.Services.Update;
using Photon.Pun;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace CodeBase.PlayerScripts
{
    public class HeroMove : MonoBehaviour, IUpdatable
    {
        [Required] [SerializeField] private CharacterController _characterController;
        [SerializeField] private float _movementSpeed = 1;

        private IInputService _inputService;
        private IUpdateService _updateService;
        private PhotonView photonView;
        private Camera _camera;
        private Vector3 _startPos;
        private Vector3 _localRight;
        private Vector3 _localForward;
        private float _dotDirection;

        [Inject]
        public void Construct(IInputService inputService, IUpdateService updateService)
        {
            _inputService = inputService;
            _updateService = updateService;
            photonView = GetComponent<PhotonView>();
            if (photonView.IsMine)
            {
                _updateService.Register(this);
                _startPos = transform.position;
            }
        }

        private void OnDisable()
        {
            if (_updateService != null)
            {
                _updateService.Unregister(this);
            }
        }

        public void Setup(Camera camera)
        {
            _camera = camera;
            _localRight = _camera.transform.right;
            _localRight.Normalize();
            _localForward = _camera.transform.up;
            _localForward.Normalize();
            Vector3 direction = transform.position - Vector3.zero;
            direction.Normalize();
            _dotDirection = Vector3.Dot(transform.forward, direction);
        }

        public void UpdateTick()
        {
            if (!_camera)
                return;
            if (!photonView || !photonView.IsMine)
                return;

            Vector3 movementAxis = Vector3.zero;
            Vector3 movementVector = Vector3.zero;


            if (_inputService.Axis.sqrMagnitude > Constants.Epsilon)
            {
                movementAxis = _inputService.Axis;
                movementAxis.z = movementAxis.y;
                movementAxis.y = 0;
                movementVector += _localForward * movementAxis.z;
                movementVector += _localRight * movementAxis.x;
                movementVector.Normalize();
            }

            _characterController.Move(_movementSpeed * movementVector * Time.deltaTime);

            Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);

            Vector2 mouseOnScreen = Camera.main.ScreenToViewportPoint(Input.mousePosition);

            float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);

            Debug.Log(_dotDirection);
            if (_dotDirection > 0.9)
            {
                angle = -angle + 90;
            }
            else
            {
                angle = -angle - 90;
            }

            transform.rotation = Quaternion.Euler(new Vector3(0f, angle, 0f));
        }

        float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
        {
            return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
        }

        public void ResetPosition()
        {
            transform.position = _startPos;
        }
    }
}